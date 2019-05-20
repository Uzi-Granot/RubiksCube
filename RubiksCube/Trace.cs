/////////////////////////////////////////////////////////////////////
//
//	Rubik's Cube for beginners.
//	WPF Application using Visual3D classes and animation.
//	Source code developed with visual C#.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: August 1, 2017
//	Copyright (C) 2017 Granotech Limited. All Rights Reserved
//
//	The Rubik's cube for beginners is a free software.
//	It is distributed under the Code Project Open License (CPOL 1.02)
//	agreement. The full text of the CPOL is given in:
//	https://www.codeproject.com/info/cpol10.aspx
//	
//	The main points of CPOL 1.02 subject to the terms of the License are:
//
//	Source Code and Executable Files can be used in commercial applications;
//	Source Code and Executable Files can be redistributed; and
//	Source Code can be modified to create derivative works.
//	No claim of suitability, guarantee, or any warranty whatsoever is
//	provided. The software is provided "as-is".
//	The Article accompanying the Work may not be distributed or republished
//	without the Author's consent
//
//	Version History:
//
//	Version 1.0 2017/08/01
//		Original revision
/////////////////////////////////////////////////////////////////////

using System;
using System.IO;

namespace UziRubiksCube
{
/////////////////////////////////////////////////////////////////////
// Trace Class
/////////////////////////////////////////////////////////////////////

static public class Trace
	{
	private static string TraceFileName;		// trace file name
	private static int MaxAllowedFileSize = 0x10000;

	/////////////////////////////////////////////////////////////////////
	// Open trace file
	/////////////////////////////////////////////////////////////////////

	public static void Open
			(
			string	FileName
			)
		{
		// save full file name
		TraceFileName = Path.GetFullPath(FileName);
		Trace.WriteTimeStamp();
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// write to trace file
	/////////////////////////////////////////////////////////////////////

	public static void Write
			(
			string Message
			)
		{
		// test file length
		TestSize();

		// open existing or create new trace file
		StreamWriter TraceFile = new StreamWriter(TraceFileName, true);

		// write message
		TraceFile.WriteLine(Message);

		// close the file
		TraceFile.Close();

		// exit
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// write to trace file
	/////////////////////////////////////////////////////////////////////

	public static void WriteTimeStamp()
		{
		// test file length
		TestSize();

		// open existing or create new trace file
		StreamWriter TraceFile = new StreamWriter(TraceFileName, true);

		// write date and time
		TraceFile.WriteLine(string.Format("---- {0:yyyy}/{0:MM}/{0:dd} {0:HH}:{0:mm}:{0:ss} ", DateTime.Now));

		// close the file
		TraceFile.Close();

		// exit
		return;
		}

	/////////////////////////////////////////////////////////////////////
	// Test file size
	// If file is too big, remove first quarter of the file
	/////////////////////////////////////////////////////////////////////

	private static void TestSize()
		{
		// get trace file info
		FileInfo TraceFileInfo = new FileInfo(TraceFileName);

		// if file does not exist or file length less than max allowed file size do nothing
		if(TraceFileInfo.Exists == false || TraceFileInfo.Length <= MaxAllowedFileSize) return;

		// create file info class
		FileStream TraceFile = new FileStream(TraceFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

		// seek to 25% length
		TraceFile.Seek(TraceFile.Length / 4, SeekOrigin.Begin);

		// new file length
		int NewFileLength = (int) (TraceFile.Length - TraceFile.Position);

		// new file buffer
		Byte[] Buffer = new Byte[NewFileLength];

		// read file to the end
		TraceFile.Read(Buffer, 0, NewFileLength);

		// search for first end of line
		int StartPtr = 0;
		while(StartPtr < 1024 && Buffer[StartPtr++] != '\n');
		if(StartPtr == 1024) StartPtr = 0;

		// seek to start of file
		TraceFile.Seek(0, SeekOrigin.Begin);

		// write 75% top part of file over the start of the file
		TraceFile.Write(Buffer, StartPtr, NewFileLength - StartPtr);

		// truncate the file
		TraceFile.SetLength(TraceFile.Position);

		// close the file
		TraceFile.Close();

		// exit
		return;
		}
	}
}
