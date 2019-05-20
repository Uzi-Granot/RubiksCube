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

namespace UziRubiksCube
{
/// <summary>
/// Edge block
/// </summary>
public class EdgeBlock
	{
	/// <summary>
	/// First face number 0-47
	/// </summary>
	public int FaceNo1;

	/// <summary>
	/// Second face number 0-47
	/// </summary>
	public int FaceNo2;

	/// <summary>
	/// First face color 0-5
	/// </summary>
	public int FaceColor1;

	/// <summary>
	/// Second face color 0-5
	/// </summary>
	public int FaceColor2;

	/// <summary>
	/// Edge block constructor
	/// </summary>
	/// <param name="FaceNo1">Face 1</param>
	/// <param name="FaceNo2">Face 2</param>
	public EdgeBlock
			(
			int FaceNo1,
			int FaceNo2
			)
		{
		this.FaceNo1 = FaceNo1;
		this.FaceNo2 = FaceNo2;
		FaceColor1 = FaceNo1 / Cube.FaceNoToColor;
		FaceColor2 = FaceNo2 / Cube.FaceNoToColor;
		return;
		}
	}
}
