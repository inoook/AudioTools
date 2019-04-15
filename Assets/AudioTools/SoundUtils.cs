using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.IO;

// http://www.peliphilo.net/archives/1545
// http://tomosoft.jp/design/?p=8985

public class SoundUtils {

	/// <summary>WAVE ヘッダ情報構造体</summary>
	public struct WaveHeaderArgs
	{
		/// <summary>RIFF ヘッダ</summary>
		public string RiffHeader;

		/// <summary>ファイルサイズ</summary>
		public int FileSize;

		/// <summary>WAVE ヘッダ</summary>
		public string WaveHeader;

		/// <summary>フォーマットチャンク</summary>
		public string FormatChunk;

		/// <summary>フォーマットチャンクサイズ</summary>
		public int FormatChunkSize;

		/// <summary>フォーマット ID</summary>
		public short FormatID;

		/// <summary>チャンネル数</summary>
		public short Channel;

		/// <summary>サンプリングレート</summary>
		public int SampleRate;

		/// <summary>1秒あたりのデータ数</summary>
		public int BytePerSec;

		/// <summary>ブロックサイズ</summary>
		public short BlockSize;

		/// <summary>1サンプルあたりのビット数</summary>
		public short BitPerSample;

		/// <summary>Data チャンク</summary>
		public string DataChunk;

		/// <summary>波形データのバイト数</summary>
		public int DataChunkSize;

		/// <summary>再生時間(msec)</summary>
		public int PlayTimeMsec;
	}

	int maxValue = 0;
	private WaveHeaderArgs waveHeader = new WaveHeaderArgs();
	private byte[] waveData;

	public bool ReadWave(string waveFilePath)
	{
		Debug.Log("ReadWave : " + waveFilePath);

		maxValue = 0;

		// ファイルの存在を確認する
		if (!File.Exists(waveFilePath))
		{
			return false;
		}

		using (FileStream fs = new FileStream(waveFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
		{
			try
			{
				BinaryReader br = new BinaryReader(fs);
				waveHeader.RiffHeader = Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));
				waveHeader.FileSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
				waveHeader.WaveHeader = Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));

				bool readFmtChunk = false;
				bool readDataChunk = false;
				while (!readFmtChunk || !readDataChunk)
				{
					// ChunkIDを取得する
					string chunk = Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));

					if (chunk.ToLower().CompareTo("fmt ") == 0)
					{
						//    Debug.WriteLine("fmt : " + waveFilePath);
						// fmtチャンクの読み込み
						waveHeader.FormatChunk = chunk;
						waveHeader.FormatChunkSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
						waveHeader.FormatID = BitConverter.ToInt16(br.ReadBytes(2), 0);
						waveHeader.Channel = BitConverter.ToInt16(br.ReadBytes(2), 0);
						waveHeader.SampleRate = BitConverter.ToInt32(br.ReadBytes(4), 0);
						waveHeader.BytePerSec = BitConverter.ToInt32(br.ReadBytes(4), 0);
						waveHeader.BlockSize = BitConverter.ToInt16(br.ReadBytes(2), 0);
						waveHeader.BitPerSample = BitConverter.ToInt16(br.ReadBytes(2), 0);

						readFmtChunk = true;
					}
					else if (chunk.ToLower().CompareTo("data") == 0)
					{
						//   Debug.WriteLine("data : ");
						// dataチャンクの読み込み
						waveHeader.DataChunk = chunk;
						waveHeader.DataChunkSize = BitConverter.ToInt32(br.ReadBytes(4), 0);

						waveData = br.ReadBytes(waveHeader.DataChunkSize);
						//  Debug.WriteLine(string.Format("waveData : {0:X} {1:X}", waveData[0], waveData[1]));

						// 再生時間を算出する
						int bytesPerSec = waveHeader.SampleRate  * waveHeader.BlockSize;
						waveHeader.PlayTimeMsec = (int)(((double)waveHeader.DataChunkSize / (double)bytesPerSec) * 1000);
						Debug.Log(string.Format("データサイズ：{0} 再生時間 : {1}", waveHeader.DataChunkSize, waveHeader.PlayTimeMsec));
//						tbxPlay.Text = waveHeader.PlayTimeMsec.ToString() + "ms秒";
						convertWaveData();

						readDataChunk = true;
					}
					else
					{
						Debug.Log("chunk : " + chunk);
						// 不要なチャンクの読み捨て
						Int32 size = BitConverter.ToInt32(br.ReadBytes(4), 0);
						if (0 < size)
						{
							br.ReadBytes(size);
						}
					}
				}
			}
			catch
			{
				fs.Close();
				return false;
			}
		}

		return true;
	}

	int[] valuesR;
	int[] valuesL;
	//
	private void convertWaveData()
	{
		try
		{
			// 音声データの取得
			valuesR = new int[(waveHeader.DataChunkSize / waveHeader.Channel) / (waveHeader.BitPerSample / 8)];
			valuesL = new int[(waveHeader.DataChunkSize / waveHeader.Channel) / (waveHeader.BitPerSample / 8)];
			Debug.LogFormat(string.Format("valuesR.Length : {0} ", valuesR.Length));
			Debug.LogFormat(string.Format("valuesL.Length : {0} ", valuesL.Length));

			// 1標本分の値を取得
			int frameIndex = 0;
			int chanelIndex = 0;

			for (int i = 0; i < waveHeader.DataChunkSize / (waveHeader.BitPerSample / 8); i++)
			{
				byte[] data = new byte[2];
				int work = 0;

				switch (waveHeader.BitPerSample)
				{
				case 8:
					work = (int)waveData[frameIndex];
					frameIndex += 1;
					break;
				case 16:
					Array.Copy(waveData, frameIndex, data, 0, 2);
					work = (int)BitConverter.ToInt16(data, 0);
					frameIndex += 2;
					break;
				default:
					Debug.LogWarning("波形解析できません");
					break;
				}
				if (waveHeader.Channel == 1)
				{
					valuesR[i] = work;
				}
				else
				{
					if (chanelIndex == 0)
					{
						chanelIndex = 1;
						valuesR[i/2] = work;
						Debug.LogFormat(string.Format("valuesR :{0} {1:X} ", i,valuesR[i/2]));
					}
					else
					{
						chanelIndex = 0;
						valuesL[i/2] = work;
						Debug.LogFormat(string.Format("valuesL : {0:X}", valuesL[i/2]));
					}
				}
			}
			Debug.LogFormat(string.Format("maxValue : {0}", maxValue));
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}
}
