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
using System.Text;

namespace UziRubiksCube
{
/// <summary>
/// Solution steps codes
/// </summary>
public enum StepCode
	{
	WhiteEdges,
	WhiteCorners,
	MidLayer,
	YellowCross,
	YellowCornersPos,
	YellowCorners,
	YellowEdges,
	CubeIsSolved,
	}

/// <summary>
/// Rubik's Cube class
/// </summary>
public class Cube
	{
	// constants that cannot be modified
	public const int BlocksPerCube = 27;
	public const int BlocksPerFace = 9;
	public const int FaceNoToColor = 8;
	public const int MovableFaces = 48;

	// face color code
	public const int WhiteFace = 0;
	public const int BlueFace = 1;
	public const int RedFace = 2;
	public const int GreenFace = 3;
	public const int OrangeFace = 4;
	public const int YellowFace = 5;
	public const int FaceColors = 6;
	
	// rotation codes
	public const int UpCW = 0;
	public const int UpCW2 = 1;
	public const int UpCCW = 2;
	public const int FrontCW = 3;
	public const int FrontCW2 = 4;
	public const int FrontCCW = 5;
	public const int RightCW = 6;
	public const int RightCW2 = 7;
	public const int RightCCW = 8;
	public const int BackCW = 9;
	public const int BackCW2 = 10;
	public const int BackCCW = 11;
	public const int LeftCW = 12;
	public const int LeftCW2 = 13;
	public const int LeftCCW = 14;
	public const int DownCW = 15;
	public const int DownCW2 = 16;
	public const int DownCCW = 17;

	// rotation codes
	public const int WhiteCW = 0;
	public const int WhiteCW2 = 1;
	public const int WhiteCCW = 2;
	public const int BlueCW = 3;
	public const int BlueCW2 = 4;
	public const int BlueCCW = 5;
	public const int RedCW = 6;
	public const int RedCW2 = 7;
	public const int RedCCW = 8;
	public const int GreenCW = 9;
	public const int GreenCW2 = 10;
	public const int GreenCCW = 11;
	public const int OrangeCW = 12;
	public const int OrangeCW2 = 13;
	public const int OrangeCCW = 14;
	public const int YellowCW = 15;
	public const int YellowCW2 = 16;
	public const int YellowCCW = 17;
	public const int RotationCodes = 18;
	public const int RotMovesPerColor = 3;

	/// <summary>
	/// face color names
	/// </summary>
	public static readonly string[] FaceColorName = new string[]
		{
		"White",
		"Blue",
		"Red",
		"Green",
		"Orange",
		"Yellow",
		"Black"
		};

	/// <summary>
	/// save solution headers
	/// </summary>
	public static readonly string[] SaveSolutionHeader = new string[]
		{
		"White  Red",
		"Blue   Yellow",
		"Red    Yellow",
		"Green  Yellow",
		"Orange Yellow",
		"Yellow Orange",
		};

	/// <summary>
	/// Relative rotations names
	/// </summary>
	public static readonly string[] RelativeRotationName = new string[]
		{
		"U",
		"U2",
		"U'",

		"F",
		"F2",
		"F'",

		"R",
		"R2",
		"R'",

		"B",
		"B2",
		"B'",

		"L",
		"L2",
		"L'",

		"D",
		"D2",
		"D'",
		};

	/// <summary>
	/// Color rotations names
	/// </summary>
	public static readonly string[] ColorRotationName = new string[]
		{
		"W",
		"W2",
		"W'",

		"B",
		"B2",
		"B'",

		"R",
		"R2",
		"R'",

		"G",
		"G2",
		"G'",

		"O",
		"O2",
		"O'",

		"Y",
		"Y2",
		"Y'",
		};

	/// <summary>
	/// block face number as function of block number and color code
	/// movable faces are coded 0 to 47
	/// invisible faces are coded as -1
	/// fixed faces are coded 48 and up
	/// </summary>
	public static readonly int[,] BlockFace = new int[,]
		{
		// W   B   R   G   O   Y            Z   Y   X
		{  0, 12, 22, -1, -1, -1}, // 0		0	0	0	W0	B4	R6	--	--	--
		{  1, -1, 21, -1, -1, -1}, // 1		0	0	1	W1	--	R5	--	--	--
		{  2, -1, 20, 30, -1, -1}, // 2		0	0	2	W2	--	R4	G6	--	--
		{  7, 13, -1, -1, -1, -1}, // 3		0	1	0	W7	B5	--	--	--	--
		{ 48, -1, -1, -1, -1, -1}, // 4		0	1	1	White					
		{  3, -1, -1, 29, -1, -1}, // 5		0	1	2	W3	--	--	G5	--	--
		{  6, 14, -1, -1, 36, -1}, // 6		0	2	0	W6	B6	--	--	O4	--
		{  5, -1, -1, -1, 37, -1}, // 7		0	2	1	W5	--	--	--	O5	--
		{  4, -1, -1, 28, 38, -1}, // 8		0	2	2	W4	--	--	G4	O6	--
		{ -1, 11, 23, -1, -1, -1}, // 9		1	0	0	--	B3	R7	--	--	--
		{ -1, -1, 50, -1, -1, -1}, // 10	1	0	1	--	--	Red	--	--	--
		{ -1, -1, 19, 31, -1, -1}, // 11	1	0	2	--	--	R3	G7	--	--
		{ -1, 49, -1, -1, -1, -1}, // 12	1	1	0	--	Blue--	--	--	--	
		{ -1, -1, -1, -1, -1, -1}, // 13	1	1	1						
		{ -1, -1, -1, 51, -1, -1}, // 14	1	1	2	--	--	--	Green-	--	
		{ -1, 15, -1, -1, 35, -1}, // 15	1	2	0	--	B7	--	--	O3	--
		{ -1, -1, -1, -1, 52, -1}, // 16	1	2	1	--	--	--	--	Orange	
		{ -1, -1, -1, 27, 39, -1}, // 17	1	2	2	--	--	--	G3	O7	--
		{ -1, 10, 16, -1, -1, 46}, // 18	2	0	0	--	B2	R0	--	--	Y7
		{ -1, -1, 17, -1, -1, 45}, // 19	2	0	1	--	--	R1	--	--	Y6
		{ -1, -1, 18, 24, -1, 44}, // 29	2	0	2	--	--	R2	G0	--	Y4
		{ -1,  9, -1, -1, -1, 47}, // 21	2	1	0	--	B1	--	--	--	Y8
		{ -1, -1, -1, -1, -1, 53}, // 22	2	1	1	--	--	--	--	--	Yellow
		{ -1, -1, -1, 25, -1, 43}, // 23	2	1	2	--	--	--	G1	--	Y3
		{ -1,  8, -1, -1, 34, 40}, // 24	2	2	0	--	B0	--	--	O2	Y0
		{ -1, -1, -1, -1, 33, 41}, // 25	2	2	1	--	--	--	--	O1	Y1
		{ -1, -1, -1, 26, 32, 42}, // 26	2	2	2				G2	O0	Y2
		};

	/// <summary>
	/// rotation vector for each possible rotation
	/// the missing vectors are calculated in static constructor
	/// </summary>
	public static readonly int[][] RotMatrix = new int[][]
		{
		new int[] {2,3,4,5,6,7,0,1,8,9,10,11,20,21,22,15,16,17,18,19,28,29,30,23,24,25,26,27,36,37,38,31,32,33,34,35,12,13,14,39,40,41,42,43,44,45,46,47},
		null,
		null,
		new int[] {36,1,2,3,4,5,34,35,10,11,12,13,14,15,8,9,0,17,18,19,20,21,6,7,24,25,26,27,28,29,30,31,32,33,46,47,40,37,38,39,16,41,42,43,44,45,22,23},
		null,
		null,
		new int[] {10,11,12,3,4,5,6,7,8,9,44,45,46,13,14,15,18,19,20,21,22,23,16,17,2,25,26,27,28,29,0,1,32,33,34,35,36,37,38,39,40,41,42,43,30,31,24,47},
		null,
		null,
		new int[] {0,1,18,19,20,5,6,7,8,9,10,11,12,13,14,15,16,17,42,43,44,21,22,23,26,27,28,29,30,31,24,25,4,33,34,35,36,37,2,3,40,41,38,39,32,45,46,47},
		null,
		null,
		new int[] {0,1,2,3,26,27,28,7,6,9,10,11,12,13,4,5,16,17,18,19,20,21,22,23,24,25,40,41,42,29,30,31,34,35,36,37,38,39,32,33,14,15,8,43,44,45,46,47},
		null,
		null,
		new int[] {0,1,2,3,4,5,6,7,32,33,34,11,12,13,14,15,8,9,10,19,20,21,22,23,16,17,18,27,28,29,30,31,24,25,26,35,36,37,38,39,42,43,44,45,46,47,40,41},
		null,
		null,
		};

	/// <summary>
	/// translation between alrorithms notations (U, F, R, B, L and D) to face colors
	/// </summary>
	public static readonly int[][] RelativeToColor = new int[][]
		{
		//				up		  front	     right		  back		  left	    down
		new int[] {WhiteFace,   BlueFace, OrangeFace,  GreenFace,    RedFace, YellowFace},
		new int[] {WhiteFace,    RedFace,   BlueFace, OrangeFace,  GreenFace, YellowFace},
		new int[] {WhiteFace,  GreenFace,    RedFace,   BlueFace, OrangeFace, YellowFace},
		new int[] {WhiteFace, OrangeFace,  GreenFace,    RedFace,   BlueFace, YellowFace},
		new int[] {YellowFace,  BlueFace,    RedFace,  GreenFace, OrangeFace, WhiteFace},
		new int[] {YellowFace,   RedFace,  GreenFace, OrangeFace,   BlueFace, WhiteFace},
		new int[] {YellowFace, GreenFace, OrangeFace,   BlueFace,    RedFace, WhiteFace},
		new int[] {YellowFace, OrangeFace,  BlueFace,    RedFace,  GreenFace, WhiteFace},
		};

	/// <summary>
	/// translation between color notations to relative rotations (U, F, R, B, L and D)
	/// </summary>
	public static readonly int[][] ColorToRelative = new int[][]
		{
		//		  White		Blue	    Red			Green		Orange		Yellow
		new int[] {UpCW,	FrontCW,	LeftCW,		BackCW, 	RightCW,	DownCW},
		new int[] {UpCW,	RightCW,	FrontCW,	LeftCW, 	BackCW,		DownCW},
		new int[] {UpCW,	BackCW,		RightCW,	FrontCW, 	LeftCW,		DownCW},
		new int[] {UpCW,	LeftCW, 	BackCW,		RightCW, 	FrontCW,	DownCW},
		new int[] {DownCW,	FrontCW,	RightCW,	BackCW, 	LeftCW,		UpCW},
		new int[] {DownCW,	LeftCW,		FrontCW,	RightCW, 	RightCW,	UpCW},
		new int[] {DownCW,	BackCW,		LeftCW, 	FrontCW, 	RightCW,	UpCW},
		new int[] {DownCW,	RightCW,	BackCW, 	LeftCW, 	FrontCW,	UpCW},
		};

	/// <summary>
	/// solution steps text
	/// </summary>
	public static readonly string[] StepCodeName =
		{
		"White Edges",
		"White Corners",
		"Mid Layer Edges",
		"Yellow Cross",
		"Yellow Corners Position",
		"Yellow Corners",
		"Yellow Edges",
		"Cube is Solved"
		};

	/// <summary>
	/// list of all face numbers for corners -1 are on white face and >= 0 on yellow
	/// </summary>
	public static readonly int[] WhiteCornerIndex = new int[]
		{
		-1, -1, -1, -1,  4,  1, -1, -1,  5,  2, 
		-1, -1,  6,  3, -1, -1,  7,  0, -1, -1, 
		 8, 11, 10,  9, 
		};

	/// <summary>
	/// White corners solution cases
	/// </summary>
	public static readonly StepCtrl[] WhiteCornerCases = new StepCtrl[]
		{
		new StepCtrl(WhiteFace, RightCCW, DownCCW, RightCW),
		new StepCtrl(WhiteFace, FrontCW, DownCW, FrontCCW),
		new StepCtrl(WhiteFace, RightCCW, DownCW2, RightCW, DownCW, RightCCW, DownCCW, RightCW),
		};

	/// <summary>
	/// Mid layer left case
	/// </summary>
	public static readonly StepCtrl MidLayerLeft =
		new StepCtrl(YellowFace, UpCCW, LeftCCW, UpCW, LeftCW, UpCW, FrontCW, UpCCW, FrontCCW);

	/// <summary>
	/// Mid layer right case
	/// </summary>
	public static readonly StepCtrl MidLayerRight =
		new StepCtrl(YellowFace, UpCW, RightCW, UpCCW, RightCCW, UpCCW, FrontCCW, UpCW, FrontCW);

	/// <summary>
	/// Yellow cross line case
	/// </summary>
	public static readonly StepCtrl YellowCrossLineCase =
		new StepCtrl(YellowFace, FrontCW, RightCW, UpCW, RightCCW, UpCCW, FrontCCW);

	/// <summary>
	/// Yellow croll L shape case
	/// </summary>
	public static readonly StepCtrl YellowCrossLShapeToCrossCase =
		new StepCtrl(YellowFace, FrontCW, UpCW, RightCW, UpCCW, RightCCW, FrontCCW);

	/// <summary>
	/// Yellow corner position left case
	/// </summary>
	public static readonly StepCtrl YellowCornerPosLeft =
		new StepCtrl(YellowFace, LeftCW, RightCCW, UpCCW, RightCW, UpCW, LeftCCW, UpCCW, RightCCW, UpCW, RightCW);

	/// <summary>
	/// Yellow corner position right case
	/// </summary>
	public static readonly StepCtrl YellowCornerPosRight =
		new StepCtrl(YellowFace, RightCCW, UpCCW, RightCW, UpCW, LeftCW, UpCCW, RightCCW, UpCW, RightCW, LeftCCW);

	/// <summary>
	/// Yellow corner orientation left case
	/// </summary>
	public static readonly StepCtrl YellowCornerOrientLeft =
		new StepCtrl(YellowFace, LeftCW, UpCW, LeftCCW, UpCW, LeftCW, UpCW2, LeftCCW, UpCW2);

	/// <summary>
	/// Yellow corner orientation right case
	/// </summary>
	public static readonly StepCtrl YellowCornerOrientRight =
		new StepCtrl(YellowFace, RightCCW, UpCCW, RightCW, UpCCW, RightCCW, UpCW2, RightCW, UpCW2);

	/// <summary>
	/// Yellow edge orientation left case
	/// </summary>
	public static readonly StepCtrl YellowEdgeLeft =
		new StepCtrl(YellowFace, RightCW, UpCCW, RightCW, UpCW, RightCW, UpCW, RightCW, UpCCW, RightCCW, UpCCW, RightCW2);

	/// <summary>
	/// Yellow edge orientation right case
	/// </summary>
	public static readonly StepCtrl YellowEdgeRight =
		new StepCtrl(YellowFace, RightCW2, UpCW, RightCW, UpCW, RightCCW, UpCCW, RightCCW, UpCCW, RightCCW, UpCW, RightCCW);

	/// <summary>
	/// Block name
	/// </summary>
	public static readonly string[] BlockName;

	/// <summary>
	/// Translate face number to block number
	/// </summary>
	public static readonly int[] FaceNoToBlockNo;

	/// <summary>
	/// Array of all edge blocks
	/// </summary>
	public static readonly EdgeBlock[] EdgeBlockArray;

	/// <summary>
	/// Array of all corner blocks
	/// </summary>
	public static readonly CornerBlock[] CornerBlockArray;

	/// <summary>
	/// array of pairs of face numbers per edge
	/// </summary>
	public static readonly int[] EdgePairArray;

	/// <summary>
	/// Random number generator
	/// </summary>
	private static Random RandomMove = new Random();

	/// <summary>
	// Face array. The one and only variable field of the cube class
	/// </summary>
	private int[] FaceArray;

	/// <summary>
	/// Static constructor
	/// </summary>
	static Cube()
		{
		// add CW2 and CCW to rotation matrix
		for(int RotateIndex = 0; RotateIndex < RotationCodes; RotateIndex += 3)
			{
			int[] M1 = RotMatrix[RotateIndex];
			int[] M2 = new int[MovableFaces];
			int[] M3 = new int[MovableFaces];
			for(int Index = 0; Index < MovableFaces; Index++) 
				{
				int R1 = M1[Index];
				int R2 = M1[R1];
				int R3 = M1[R2];
				M2[Index] = R2;
				M3[Index] = R3;
				}

			RotMatrix[RotateIndex + 1] = M2;
			RotMatrix[RotateIndex + 2] = M3;
			}

		// face number to block number
		FaceNoToBlockNo = new int[MovableFaces];
		for(int Block = 0; Block < BlocksPerCube; Block++)
			for(int Face = 0; Face < FaceColors; Face++)
				{
				int FaceNo = BlockFace[Block, Face];
				if(FaceNo >= 0 && FaceNo < MovableFaces) FaceNoToBlockNo[FaceNo] = Block;
				}

		// block name
		BlockName = new string[BlocksPerCube];
		for(int BlockNo = 0; BlockNo < BlocksPerCube; BlockNo++)
			{
			StringBuilder Name = new StringBuilder();
			if(BlockFace[BlockNo, WhiteFace] >= 0 && BlockFace[BlockNo, WhiteFace] < MovableFaces)
				Name.Append(FaceColorName[WhiteFace] + " ");
			if(BlockFace[BlockNo, YellowFace] >= 0 && BlockFace[BlockNo, YellowFace] < MovableFaces)
				Name.Append(FaceColorName[YellowFace] + " ");
			for(int FaceColor = BlueFace; FaceColor < YellowFace; FaceColor++)
				{
				if(BlockFace[BlockNo, FaceColor] >= 0 && BlockFace[BlockNo, FaceColor] < MovableFaces)
					Name.Append(FaceColorName[FaceColor] + " ");
				}
			if(Name.Length > 0)
				{
				Name.Append((BlockNo & 1) == 0 ? "Corner" : "Edge");
				BlockName[BlockNo] = Name.ToString();
				}
			}

		// Edge block array
		EdgeBlockArray = new EdgeBlock[12];
		EdgePairArray = new int[24];
		int Ptr = 0;
		for(int BlockNo = 1; BlockNo < BlocksPerCube; BlockNo += 2)
			{
			int FaceNo1 = -1;
			int FaceNo2 = -1;
			for(int FaceColor = WhiteFace; FaceColor <= YellowFace; FaceColor++)
				{
				int FaceNo = BlockFace[BlockNo, FaceColor];
				if(FaceNo >= 0 && FaceNo < MovableFaces)
					{
					if(FaceNo1 < 0) FaceNo1 = FaceNo;
					else FaceNo2 = FaceNo;
					}
				}

			// movable block
			if(FaceNo1 >= 0 && FaceNo2 >= 0)
				{
				EdgeBlockArray[Ptr++] = new EdgeBlock(FaceNo1, FaceNo2);
				EdgePairArray[FaceNo1 / 2] = FaceNo2;
				EdgePairArray[FaceNo2 / 2] = FaceNo1;
				}
			}

		// corner block array
		CornerBlockArray = new CornerBlock[8];
		Ptr = 0;
		for(int BlockNo = 0; BlockNo < BlocksPerCube; BlockNo += 2)
			{
			int FaceNo1 = -1;
			int FaceNo2 = -1;
			int FaceNo3 = -1;
			for(int FaceColor = WhiteFace; FaceColor <= YellowFace; FaceColor++)
				{
				int FaceNo = BlockFace[BlockNo, FaceColor];
				if(FaceNo >= 0 && FaceNo < MovableFaces)
					{
					if(FaceNo1 < 0) FaceNo1 = FaceNo;
					else if(FaceNo2 < 0) FaceNo2 = FaceNo;
					else FaceNo3 = FaceNo;
					}
				}
			if(FaceNo1 >= 0 && FaceNo2 >= 0 && FaceNo3 >= 0) CornerBlockArray[Ptr++] = new CornerBlock(FaceNo1, FaceNo2, FaceNo3);
			}

		return;
		}

	/// <summary>
	/// Constructor
	/// </summary>
	public Cube()
		{
		Reset();
		return;
		}

	/// <summary>
	/// Copy constructor
	/// </summary>
	/// <param name="BaseCube">The cube to be copied</param>
	public Cube
			(
			Cube BaseCube
			)
		{
		FaceArray = (int[]) BaseCube.FaceArray.Clone();
		return;
		}

	/// <summary>
	/// Reset cube to solved state
	/// </summary>
	public void Reset()
		{
		FaceArray = new int[MovableFaces];
		for(int Index = 0; Index < MovableFaces; Index++) FaceArray[Index] = Index;
		return;
		}

	/// <summary>
	/// Get color of one face
	/// </summary>
	/// <param name="FaceNo">Face number</param>
	/// <returns>Face color code</returns>
	public int FaceColor
			(
			int FaceNo
			)
		{
		return FaceArray[FaceNo] / FaceNoToColor;
		}

	/// <summary>
	/// Reset user color array to current cube
	/// </summary>
	public int[] ColorArray
		{
		get
			{
			int[] ColorArray = new int[MovableFaces];			
			for(int FaceNo = 0; FaceNo < MovableFaces; FaceNo++) ColorArray[FaceNo] = FaceColor(FaceNo);
			return ColorArray;
			}
		set
			{
			FaceArray = TestUserColorArray(value);
			return;
			}
		}

	/// <summary>
	/// Test for solved state
	/// </summary>
	/// <returns>Solved state</returns>
	public bool AllBlocksInPlace
		{
		get
			{
			for(int Index = 0; Index < MovableFaces; Index++) if(FaceArray[Index] != Index) return false;
			return true;
			}
		}

	/// <summary>
	/// All white edges are in a solved stste
	/// </summary>
	public bool AllWhiteEdgesInPlace
		{
		get
			{
			return FaceArray[1] == 1 && FaceArray[3] == 3 && FaceArray[5] == 5 && FaceArray[7] == 7;
			}
		}

	/// <summary>
	/// All white corners are in a solved state
	/// </summary>
	public bool AllWhiteCornersInPlace
		{
		get
			{
			return FaceArray[0] == 0 && FaceArray[2] == 2 && FaceArray[4] == 4 && FaceArray[6] == 6;
			}
		}

	/// <summary>
	/// All mid-layer edges are in a solved state
	/// </summary>
	public bool AllMidLayerEdgesInPlace
		{
		get
			{
			return FaceArray[11] == 11 && FaceArray[19] == 19 && FaceArray[27] == 27 && FaceArray[35] == 35;
			}
		}

	/// <summary>
	/// Yellow edges are in a cross shape
	/// </summary>
	public bool YellowEdgesInCrossShape
		{
		get
			{
			return FaceColor(41) == YellowFace && FaceColor(43) == YellowFace && FaceColor(45) == YellowFace && FaceColor(47) == YellowFace;
			}
		}

	/// <summary>
	/// All yellow edges are in solved state
	/// </summary>
	public bool AllYellowEdgesInPlace
		{
		get
			{
			return FaceArray[41] == 41 && FaceArray[43] == 43 && FaceArray[45] == 45 && FaceArray[47] == 47;
			}
		}

	/// <summary>
	/// All yellow corners are in their position but not neccessarily in solved state
	/// </summary>
	public bool AllYellowCornersInPosition
		{
		get
			{
			return FaceNoToBlockNo[40] == FaceNoToBlockNo[FaceArray[40]] &&
				FaceNoToBlockNo[42] == FaceNoToBlockNo[FaceArray[42]] &&
				FaceNoToBlockNo[44] == FaceNoToBlockNo[FaceArray[44]] &&
				FaceNoToBlockNo[46] == FaceNoToBlockNo[FaceArray[46]];
			}
		}

	/// <summary>
	/// All yellow corners are in solved state
	/// </summary>
	public bool AllYellowCornersInPlace
		{
		get
			{
			return FaceArray[40] == 40 && FaceArray[42] == 42 && FaceArray[44] == 44 && FaceArray[46] == 46;
			}
		}

	/// <summary>
	/// Rotate Array
	/// </summary>
	/// <param name="RotationCode">Color rotation code</param>
	public void RotateArray
			(
			int RotationCode
			)
		{
		FaceArray = RotateArray(FaceArray, RotationCode);
		return;
		}

	/// <summary>
	/// Rotate array
	/// </summary>
	/// <param name="RotationSteps">Rotation steps array</param>
	public void RotateArray
			(
			int[] RotationSteps
			)
		{
		foreach(int RotateCode in RotationSteps) RotateArray(RotateCode);
		return;
		}

	/// <summary>
	/// Next solution step
	/// </summary>
	/// <returns>Solve step resutl</returns>
	public SolutionStep NextSolutionStep()
		{
		if(!AllWhiteEdgesInPlace) return SolveWhiteEdges();
		else if(!AllWhiteCornersInPlace) return SolveWhiteCorners();
		else if(!AllMidLayerEdgesInPlace) return SolveMidLayerEdges();
		else if(!YellowEdgesInCrossShape) return SolveYellowEdgesCrossShape();
		else if(!AllYellowCornersInPosition) return SolveYellowCornersPosition();
		else if(!AllYellowCornersInPlace) return SolveYellowCornersOrientation();
		else if(!AllBlocksInPlace) return SolveYellowEdgesOrientation();
		return new SolutionStep();
		}

	/// <summary>
	/// Solve white edges
	/// </summary>
	/// <returns>Solve step</returns>
	private SolutionStep SolveWhiteEdges()
		{
		// test if we have at least one white edge on the white face
		int[] TempFaceArray = FaceArray;
		int BestCount = 0;
		int BestRot = 0;
		int BestFaceNo = 0;
		for(int Rotation = 0; Rotation < 4; Rotation++)
			{
			// roate white face CW
			if(Rotation > 0) TempFaceArray = RotateArray(TempFaceArray, WhiteCW);

			// count how many white edges are in the right place
			int SaveFaceNo = 0;
			int Count = 0;
			for(int FaceNo = 1; FaceNo < 9; FaceNo += 2) if(TempFaceArray[FaceNo] == FaceNo)
				{
				Count++;
				SaveFaceNo = FaceNo;
				}

			// save best count
			if(Count > BestCount)
				{
				BestCount = Count;
				BestRot = Rotation;
				BestFaceNo = SaveFaceNo;
				}
			}

		// we have at least one white edge on the white face
		// and we need to rotate to get it in place
		// find the first match
		if(BestCount > 0 && BestRot > 0)
			{
			// the other face color of the white edge
			return new SolutionStep(StepCode.WhiteEdges, "Rotate to position", BestFaceNo, WhiteFace,
				OtherEdgeColor(BestFaceNo), new int[] {BestRot - 1});
			}

		// white edge in place array
		bool[] WhiteEdges = new bool[4];
		WhiteEdges[0] = FaceArray[1] == 1;
		WhiteEdges[1] = FaceArray[3] == 3;
		WhiteEdges[2] = FaceArray[5] == 5;
		WhiteEdges[3] = FaceArray[7] == 7;

		// try single rotation to get one more edge in place
		for(int R1 = BlueCW; R1 < YellowCW; R1++)
			{
			SolutionStep Step = TestWhiteEdges(WhiteEdges, new int[] {R1});
			if(Step != null) return Step;
			}

		// try all possible double rotations to get one more edge in place
		for(int R1 = 0; R1 < RotationCodes; R1++)
			{
			for(int R2 = 0; R2 < RotationCodes; R2++)
				{
				if(R1 / 3 == R2 / 3) continue;
				SolutionStep Step = TestWhiteEdges(WhiteEdges, new int[] {R1, R2});
				if(Step != null) return Step;
				}
			}

		// try all possible three rotations to get one more edge in place
		for(int R1 = 0; R1 < RotationCodes; R1++)
			{
			for(int R2 = 0; R2 < RotationCodes; R2++)
				{
				if(R1 / 3 == R2 / 3) continue;
				for(int R3 = 0; R3 < RotationCodes; R3++)
					{
					if(R2 / 3 == R3 / 3) continue;
					SolutionStep Step = TestWhiteEdges(WhiteEdges, new int[] {R1, R2, R3});
					if(Step != null) return Step;
					}
				}
			}

		// try all possible four rotations to get one more edge in place
		for(int R1 = 0; R1 < RotationCodes; R1++)
			{
			for(int R2 = 0; R2 < RotationCodes; R2++)
				{
				if(R1 / 3 == R2 / 3) continue;
				for(int R3 = 0; R3 < RotationCodes; R3++)
					{
					if(R2 / 3 == R3 / 3) continue;
					for(int R4 = 0; R4 < RotationCodes; R4++)
						{
						if(R3 / 3 == R4 / 3) continue;
						SolutionStep Step = TestWhiteEdges(WhiteEdges, new int[] {R1, R2, R3, R4});
						if(Step != null) return Step;
						}
					}
				}
			}

		throw new ApplicationException("Solve white edges. Four rotations is not enough.");
		}

	/// <summary>
	/// Test if one more white edge is in place
	/// </summary>
	/// <param name="WhiteEdges">White edges in place control array</param>
	/// <param name="Steps">Ritation steps</param>
	/// <returns>Solve step</returns>
	private SolutionStep TestWhiteEdges
			(
			bool[] WhiteEdges,
			int[] Steps
			)
		{
		// rotate
		int[] TestArray = RotateArray(FaceArray, Steps);

		// make sure that white edges that were in place are still in place
		if(WhiteEdges[0] && TestArray[1] != 1 || WhiteEdges[1] && TestArray[3] != 3 ||
			WhiteEdges[2] && TestArray[5] != 5 || WhiteEdges[3] && TestArray[7] != 7) return null;

		// test if one edge that was not in place moved to its place
		for(int FaceNo = 1; FaceNo < 9; FaceNo += 2)
			{
			if(!WhiteEdges[FaceNo / 2] && TestArray[FaceNo] == FaceNo)
				{
				int FrontFace = RedFace + FaceNo / 2;
				if(FrontFace > OrangeFace) FrontFace = BlueFace;
				return new SolutionStep(StepCode.WhiteEdges, "Move to position", FaceNo, WhiteFace, FrontFace, Steps);
				}
			}

		// test failed
		return null;
		}

	/// <summary>
	/// Solve white corners
	/// </summary>
	/// <returns>Solve step</returns>
	private SolutionStep SolveWhiteCorners()
		{
		// find the current location of the 4 white corners
		WhiteCorner[] CornerArray = new WhiteCorner[4];

		// find a corner on the yellow side with no rotation
		for(int Index = 0; Index < 4; Index++)
			{
			// create white corner object
			WhiteCorner Corner = WhiteCorner.Create(FaceArray, 2 * Index);

			// corner is in place
			if(Corner == null) continue;

			// corner is in yellow face with zero rotation
			if(!Corner.MoveToYellow && Corner.YellowRotation == 0) return Corner.CreateSolutionStep("Move to position");

			// save in array
			CornerArray[Index] = Corner;
			}

		// find a corner on the yellow side with rotation
		foreach(WhiteCorner Corner in CornerArray)
			{
			// corner is in place or in white face but wrong position
			if(Corner != null && !Corner.MoveToYellow)  return Corner.CreateSolutionStep("Rotate and move to position");
			}

		// we have a white corner on the white side with wrong position or orientation
		// we need to take it out of the white face and move it to the yellow face
		foreach(WhiteCorner Corner in CornerArray)
			{
			// corner is in place or in white face but wrong position
			if(Corner != null)  return Corner.CreateSolutionStep("Move to yellow face");
			}

		// error
		throw new ApplicationException("Solve white corners. Invalid cube.");
		}

	/// <summary>
	/// Solve mid layer edges
	/// </summary>
	/// <returns>Solve step</returns>
	private SolutionStep SolveMidLayerEdges()
		{
		// 4 mid layer edges
		MidLayer[] MidLayerArray = new MidLayer[4];

		// move edge with right or left algorithm removing initial yellow rotation
		for(int Index = 0; Index < 4; Index++)
			{
			// create mid layer edge object
			MidLayer Edge = MidLayer.Create(FaceArray, 11 + 8 * Index);

			// look for move edge with no initial step
			if(Edge != null && !Edge.MoveToYellow && Edge.YellowRotation == 0) return Edge.CreateSolutionStep1("Move to position (remove first step)");

			// save object
			MidLayerArray[Index] = Edge;
			}

		// move edge with right or left algorithm with yellow face rotation
		foreach(MidLayer Edge in MidLayerArray)
			{
			// look for move edge from yellow face
			if(Edge != null && !Edge.MoveToYellow)
				{
				if(Edge.Rotation == 0) return Edge.CreateSolutionStep2("Move to position");
				return Edge.CreateSolutionStep3("Move to position (adjust first step)");
				}
			}

		// move edge out of middle layer with right algorithm
		// less the first yellow face move
		foreach(MidLayer Edge in MidLayerArray)
			{
			if(Edge != null) return Edge.CreateSolutionStep4("Move edge to yellow face");
			}

		// error
		throw new ApplicationException("Solve mid layer edges. Invalid cube.");
		}

	/// <summary>
	/// Solve yellow edges cross 
	/// </summary>
	/// <returns>Solve step</returns>
	private SolutionStep SolveYellowEdgesCrossShape()
		{
		// look for first yellow edge face
		int YelEdge1;
		for(YelEdge1 = 41; YelEdge1 < 49 && FaceColor(YelEdge1) != YellowFace; YelEdge1 += 2);

		// no yellow edge is yellow
		if(YelEdge1 == 49) return new SolutionStep(StepCode.YellowCross, "Move first two yellow faces", 45,
			YellowFace, GreenFace, YellowCrossLineCase.Steps(2));

		// look for second yellow edge face
		int YelEdge2;
		for(YelEdge2 = YelEdge1 + 2; YelEdge2 < 49 && FaceColor(YelEdge2) != YellowFace; YelEdge2 += 2);

		// there must be two
		if(YelEdge1 == 49) throw new ApplicationException("Solve yellow cross. Invalid cube. Two yellow faces.");

		// two yellow edges form a line 41 to 45 (green)
		if(YelEdge1 == 41 && YelEdge2 == 45)
			return new SolutionStep(StepCode.YellowCross, "Move from line to cross", 43,
				YellowFace, GreenFace, YellowCrossLineCase.Steps(2));

		// two yellow edges form a line 43 to 47 (red)
		if(YelEdge1 == 43 && YelEdge2 == 47)
			return new SolutionStep(StepCode.YellowCross, "Move from line to cross", 45,
				YellowFace, RedFace, YellowCrossLineCase.Steps(1));

		// we have an L shape
		// the algorithm goes fro L shape to cross
		int Index;
		if(YelEdge1 == 41)
			{
			if(YelEdge2 == 43) Index = 0; // 41-43 Blue
			else Index = 1; // 41-47 Red
			}
		else if(YelEdge1 == 43) Index = 3; // 43-45 Orange
		else Index = 2; // 45-47 Green

		// two yellow edges form an L shape
		return new SolutionStep(StepCode.YellowCross, "Move from L shape to cross", 47 - 2 * Index,
			YellowFace, Index + 1, YellowCrossLShapeToCrossCase.Steps(Index));
		}

	/// <summary>
	/// Solve yellow corners to correct position but not neccessarily correct orientation
	/// </summary>
	/// <returns>Solve step</returns>
	private SolutionStep SolveYellowCornersPosition()
		{
		// rotate the yellow face until there is only 1 corner or 4 corners in the correct place
		// note count=4 and rotation=0 is all corners are in their correct position
		int[] TempFaceArray = FaceArray;
		int Count;
		int Rotate = 0;
		int Match = 0;
		int Match2 = 0;
		int Rotate2 = 0;
		for(;;)
			{
			Count = 0;
			for(int Index = 40; Index < MovableFaces; Index += 2)
				{
				if(FaceNoToBlockNo[Index] == FaceNoToBlockNo[TempFaceArray[Index]])
					{
					Count++;
					Match = Index;
					if(Count == 2 && Match2 == 0)
						{
						Match2 = Match;
						Rotate2 = Rotate;
						}
					}
				}
			if(Count == 1 || Count == 4 || Rotate == 3) break;
			TempFaceArray = RotateArray(TempFaceArray, YellowCW);
			Rotate++;
			}

		// we have 4 matches we need just rotation to get all into position
		if(Count == 4) return new SolutionStep(StepCode.YellowCornersPos, "Rotate yellow face to position", 44,
			YellowFace, RedFace, new int[] {YellowCW + Rotate - 1});

		// we have one corner match and three corners not in position
		if(Count == 1)
			{
			// next corner CW after the match
			int MatchR = Match + 2;
			if(MatchR > 46) MatchR = 40;

			// next corner CCW after the match
			int MatchL = Match - 2;
			if(MatchL < 40) MatchL = 46;

			// use left algorithm to move the corner in position MatchR to position MatchL
			// use right algorithm to move the corner in position MatchL to position MatchR
			StepCtrl Step = FaceNoToBlockNo[MatchL] == FaceNoToBlockNo[TempFaceArray[MatchR]] ? YellowCornerPosLeft : YellowCornerPosRight;
			
			// return
			return SolveYellowCornersPosition(Step, Match, Rotate, MatchR, "Rotate 3 corners into position");
			}

		// the only other possible case is Count == 2
		// if case 2 was not found the cube is invalid
		if(Match2 == 0) throw new ApplicationException("Solve yellow corners position. Invalid cube.");

		// get left algorthm that does not include the match 2 block
		StepCtrl Step2 = YellowCornerPosLeft;

		// return
		return SolveYellowCornersPosition(YellowCornerPosLeft, Match2, Rotate, Match2 > 44 ? 40 : Match2 + 2, "Rotate 3 corners to get one corner match");
		// we have two corners and we need to shuffle the corners such that we will have one
		}

	/// <summary>
	/// Solve yellow corner posion helper method
	/// </summary>
	/// <param name="Step">Step control</param>
	/// <param name="Match">Yellow corner face number in correct spot</param>
	/// <param name="Rotate">Rotation to bring the face to right spot</param>
	/// <param name="FaceNo">Face number</param>
	/// <param name="Message">Message</param>
	/// <returns></returns>
	private SolutionStep SolveYellowCornersPosition
			(
			StepCtrl Step,
			int Match,
			int Rotate,
			int FaceNo,
			string Message
			)
		{
		// calculate index to algorithm
		int Index = 0;
		switch(Match)
			{
			// green
			case 40:
				Index = 2;
				break;

			// red
			case 42:
				Index = 1;
				break;

			// blue
			case 44:
				Index = 0;
				break;

			// orange
			case 46:
				Index = 3;
				break;
			}

		// combine rotation and algorithm to one solve step
		int[] Steps = Step.Steps(Index);
		if(Rotate != 0)
			{
			int[] TempSteps = Steps;
			int Len = TempSteps.Length;
			Steps = new int[Len + 1];
			Steps[0] = YellowCW + Rotate - 1;
			Array.Copy(TempSteps, 0, Steps, 1, Len);
			}

		// return
		return new SolutionStep(StepCode.YellowCornersPos, Message, FaceNo, YellowFace, BlueFace + Index, Steps);
		}

	/// <summary>
	/// Solve yellow corners orientation
	/// </summary>
	/// <returns>Solve step</returns>
	private SolutionStep SolveYellowCornersOrientation()
		{
		// look for one and only one corner in place
		SolutionStep Step = LookForOneYellowCornerMatch(FaceArray);
		if(Step != null) return Step;

		// try all 8 possible yellow corner orientations to find one with one match
		for(int Index = 0; Index < 8; Index++)
			{
			StepCtrl StepCtrl = Index < 4 ? YellowCornerOrientLeft : YellowCornerOrientRight;
			int StepsIndex = Index % 4;
			int[] Steps = StepCtrl.Steps(StepsIndex);

			// simulate rotatiom of three corners
			int[] TempFaceArray = RotateArray(FaceArray, Steps);

			// test if one match is found
			SolutionStep Step2 = LookForOneYellowCornerMatch(TempFaceArray);

			// shuffle three yellow corners to bring one match case
			if(Step2 != null) return new SolutionStep(StepCode.YellowCorners, "Shuffle three yellow corners", Step2.FaceNo,
				YellowFace, BlueFace + StepsIndex, Steps);
			}

		throw new ApplicationException("Solve yellow corners orientation. Invalid cube.");
		}

	/// <summary>
	/// Look for one yellow corner match
	/// </summary>
	/// <param name="FaceArrayArg">Face array</param>
	/// <returns>Solve step</returns>
	private SolutionStep LookForOneYellowCornerMatch
			(
			int[] FaceArrayArg
			)
		{
		// count how many yellow corners are in place
		int Count = 0;
		int Match = 0;
		for(int FaceNo = 40; FaceNo < MovableFaces; FaceNo += 2) if(FaceArrayArg[FaceNo] == FaceNo)
			{
			Count++;
			Match = FaceNo;
			}

		// one match was not found
		if(Count != 1) return null;

		// index to yellow corner orientation algorithm
		// calculate left index to algorithm
		int Index = 0;
		switch(Match)
			{
			// green
			case 40:
				Index = 2;
				break;

			// red
			case 42:
				Index = 1;
				break;

			// blue
			case 44:
				Index = 0;
				break;

			// orange
			case 46:
				Index = 3;
				break;
			}

		// test for left or right algorithm
		bool LeftAlgo = FaceArrayArg[10 + 8 * Index] / FaceNoToColor == YellowFace;

		// for right algorithm change index
		if(!LeftAlgo)
			{
			Index--;
			if(Index < 0) Index = 3;
			}

		// load correct step control
		StepCtrl StepCtrl = LeftAlgo ? YellowCornerOrientLeft : YellowCornerOrientRight;

		// return with solution step
		return new SolutionStep(StepCode.YellowCorners, "Rotate 3 yellow corners into their place",
			Match, YellowFace, BlueFace + Index, StepCtrl.Steps(Index));
		}

	/// <summary>
	/// Rotate 3 yellow edges to their correct place
	/// </summary>
	/// <returns>Solve Step</returns>
	private SolutionStep SolveYellowEdgesOrientation()
		{
		// look for one and only one yellow edge in place
		SolutionStep Step = LookForOneYellowEdgeMatch(FaceArray);
		if(Step != null) return Step;

		// try all 8 possible yellow corner orientations to find one with one match
		for(int Index = 0; Index < 8; Index++)
			{
			StepCtrl StepCtrl = Index < 4 ? YellowEdgeLeft : YellowEdgeRight;
			int StepsIndex = Index % 4;
			int[] Steps = StepCtrl.Steps(StepsIndex);

			// simulate rotatiom of three corners
			int[] TempFaceArray = RotateArray(FaceArray, Steps);

			// test if one match is found
			SolutionStep Step2 = LookForOneYellowEdgeMatch(TempFaceArray);

			// shuffle three yellow corners to bring one match case
			if(Step2 != null) return new SolutionStep(StepCode.YellowEdges, "Shuffle yellow edges to get one match",
				Step2.FaceNo, YellowFace, BlueFace + StepsIndex, Steps);
			}

		throw new ApplicationException("Solve yellow edges orientation. Invalid cube.");
		}

	/// <summary>
	/// Look for one yellow edge match
	/// </summary>
	/// <param name="FaceArrayArg">Face array</param>
	/// <returns>Solve step</returns>
	private SolutionStep LookForOneYellowEdgeMatch
			(
			int[] FaceArrayArg
			)
		{
		// count how many edges are in place
		int Count = 0;
		int Match = 0;
		for(int FaceNo = 41; FaceNo < MovableFaces; FaceNo += 2) if(FaceArrayArg[FaceNo] == FaceNo)
			{
			Count++;
			Match = FaceNo;
			}

		// one match not found
		if(Count != 1) return null;

		// next edge CW after the match
		int MatchR = Match + 2;
		if(MatchR > MovableFaces) MatchR = 41;

		// next edge CCW after the match
		int MatchL = Match - 2;
		if(MatchL < 41) MatchL = 47;

		// algorithm index
		int Index = 0;
		switch(Match)
			{
			// red
			case 41:
				Index = 1;
				break;

			// blue
			case 43:
				Index = 0;
				break;

			// orange
			case 45:
				Index = 3;
				break;

			// green
			case 47:
				Index = 2;
				break;
			}

		// get step control
		StepCtrl StepCtrl = FaceArrayArg[MatchR] == MatchL ? YellowEdgeLeft : YellowEdgeRight;

		// return solve step
		return new SolutionStep(StepCode.YellowEdges, "Rotate 3 edges to position",
			MatchR, YellowFace, BlueFace + Index, StepCtrl.Steps(Index));
		}

	/// <summary>
	/// Reset cube to solved state
	/// </summary>
	public static int[] ResetFaceArray()
		{
		int[] FaceArray = new int[MovableFaces];
		for(int Index = 0; Index < MovableFaces; Index++) FaceArray[Index] = Index;
		return FaceArray;
		}

	/// <summary>
	/// Rotate face array by rotation code
	/// </summary>
	/// <param name="FaceArrayArg">Face array</param>
	/// <param name="RotationCode">Rotation code</param>
	/// <returns>Face array result</returns>
	public static int[] RotateArray
			(
			int[] FaceArrayArg,
			int RotationCode
			)
		{
		int[] RotateVector = RotMatrix[RotationCode];
		int[] TempFaceArray = new int[MovableFaces];
		for(int Index = 0; Index < MovableFaces; Index++) TempFaceArray[RotateVector[Index]] = FaceArrayArg[Index];
		return TempFaceArray;
		}

	/// <summary>
	/// Rotate face array by a series of rotation steps
	/// </summary>
	/// <param name="FaceArrayArg">Face array</param>
	/// <param name="RotationSteps">Rotation steps</param>
	/// <returns>Face array result</returns>
	public static int[] RotateArray
			(
			int[] FaceArrayArg,
			int[] RotationSteps
			)
		{
		int[] TempFaceArray = FaceArrayArg;
		foreach(int RotateCode in RotationSteps) TempFaceArray = RotateArray(TempFaceArray, RotateCode);
		return TempFaceArray;
		}

	/// <summary>
	/// Block namee from face number
	/// </summary>
	/// <param name="FaceNo">Face number</param>
	/// <returns>Block name</returns>
	public static string GetBlockName
			(
			int FaceNo
			)
		{
		return FaceNo >= 0 ? BlockName[FaceNoToBlockNo[FaceNo]] : string.Empty;
		}

	/// <summary>
	/// Find face position from face number (corners only)
	/// </summary>
	/// <param name="FaceArrayArg">Face array</param>
	/// <param name="FaceNo">Face number</param>
	/// <returns>Face position</returns>
	public static int FindCorner
			(
			int[] FaceArrayArg,
			int FaceNo
			)
		{
		for(int FacePos = 0; FacePos < MovableFaces; FacePos += 2) if(FaceNo == FaceArrayArg[FacePos]) return FacePos;
		throw new ApplicationException("Find corner failed");
		}

	/// <summary>
	/// Find face position from face number (edges only)
	/// </summary>
	/// <param name="FaceArrayArg">Face array</param>
	/// <param name="FaceNo">face number</param>
	/// <returns>Face position</returns>
	public static int FindEdge
			(
			int[] FaceArrayArg,
			int FaceNo
			)
		{
		for(int FacePos = 1; FacePos < MovableFaces; FacePos += 2) if(FaceNo == FaceArrayArg[FacePos]) return FacePos;
		throw new ApplicationException("Find edge failed");
		}

	/// <summary>
	/// Other edge color
	/// </summary>
	/// <param name="FaceNo">Face number</param>
	/// <returns>Other face color</returns>
	public static int OtherEdgeColor
			(
			int FaceNo
			)
		{
		return EdgePairArray[FaceNo / 2] / FaceNoToColor;
		}

	/// <summary>
	/// Test user face color array
	/// </summary>
	/// <param name="UserColorArray">Color array</param>
	/// <returns>FaceArray</returns>
	public static int[] TestUserColorArray
			(
			int[] UserColorArray
			)
		{
		// test corners
		int[] Count = new int[FaceColors];
		for(int Index = 0; Index < MovableFaces; Index += 2)
			{
			// color code at position index
			int ColorCode = UserColorArray[Index];

			// make sure color code is WhiteFace(0) to YellowFace(5)
			if(ColorCode < WhiteFace || ColorCode > YellowFace) throw new ApplicationException("Color array corner item is not valid color");

			// count how many times each color is present
			Count[ColorCode]++;
			}

		// make sure each color appears 4 times
		for(int Index = 0; Index < FaceColors; Index++) if(Count[Index] != 4)
			{
			throw new ApplicationException(string.Format("Set color error. There are too {0} {1} corner faces.",
				Count[Index] > 4 ? "many" : "little", FaceColorName[Index]));
			}

		// test edges
		Count = new int[FaceColors];
		for(int Index = 1; Index < MovableFaces; Index += 2)
			{
			// color code at position index
			int ColorCode = UserColorArray[Index];

			// make sure color code is WhiteFace(0) to YellowFace(5)
			if(ColorCode < WhiteFace || ColorCode > YellowFace) throw new ApplicationException("Color array edge item is not valid color");

			// count how many times each color is present
			Count[ColorCode]++;
			}

		// make sure each color appears 4 times
		for(int Index = 0; Index < FaceColors; Index++) if(Count[Index] != 4)
			{
			throw new ApplicationException(string.Format("Set color error. There are too {0} {1} edge faces.",
				Count[Index] > 4 ? "many" : "little", FaceColorName[Index]));
			}

		// build face array as per user
		int[] UserFaceArray = new int[MovableFaces];

		// corners
		for(int Index = 0; Index < 8; Index++) TestUserCorner(CornerBlockArray[Index], UserColorArray, UserFaceArray);

		// edges
		for(int Index = 0; Index < 12; Index++) TestUserEdge(EdgeBlockArray[Index], UserColorArray, UserFaceArray);

		// create test cube
		Cube TestCube = new Cube()
			{
			FaceArray = (int[]) UserFaceArray.Clone()
			};

		StepCode StepNo = StepCode.WhiteEdges;
		int StepCounter = 0;

		// try to solve it
		for(;;)
			{
			// get next step
			SolutionStep SolveStep = TestCube.NextSolutionStep();

			// cube is solved
			if(SolveStep.StepCode == StepCode.CubeIsSolved) break;

			// test for progress
			if(SolveStep.StepCode > StepNo)
				{
				StepNo = SolveStep.StepCode;
				StepCounter = 0;
				}

			// test for regression
			else if(SolveStep.StepCode < StepNo)
				{
				throw new ApplicationException("Invalid cube. Solution regression.");
				}

			// test for loop
			else if(StepCounter > 8)
				{
				throw new ApplicationException("Invalid cube. Solution is in a loop.");
				}

			// perform the rotation steps
			TestCube.RotateArray(SolveStep.Steps);
			StepCounter++;
			}

		// return tested face array
		return UserFaceArray;
		}

	/// <summary>
	/// Test corners
	/// </summary>
	/// <param name="CornerBlock">Corner block definition</param>
	/// <param name="UserColorArray">User color array (input)</param>
	/// <param name="UserFaceArray">User face array (output)</param>
	private static void TestUserCorner
			(
			CornerBlock CornerBlock,
			int[] UserColorArray,
			int[] UserFaceArray
			)
		{
		// three face numbers assoicated with the corner standard position
		int StandardFaceNo1 = CornerBlock.FaceNo1;
		int StandardFaceNo2 = CornerBlock.FaceNo2;
		int StandardFaceNo3 = CornerBlock.FaceNo3;

		// three face colors after the user paint the faces
		int UserFaceColor1 = UserColorArray[StandardFaceNo1];
		int UserFaceColor2 = UserColorArray[StandardFaceNo2];
		int UserFaceColor3 = UserColorArray[StandardFaceNo3];

		// make sure all colors are different
		if(UserFaceColor1 == UserFaceColor2 || UserFaceColor1 == UserFaceColor3 || UserFaceColor2 == UserFaceColor3)
			throw new ApplicationException(string.Format("Corner faces colors {0}, {1} and {2} must be all different.",
				FaceColorName[UserFaceColor1], FaceColorName[UserFaceColor2], FaceColorName[UserFaceColor3]));

		// look for the corner in the standard solved cube
		foreach(CornerBlock Corner in CornerBlockArray)
			{
			// match color1
			int FaceNo1;
			if(UserFaceColor1 == Corner.FaceColor1) FaceNo1 = Corner.FaceNo1;
			else if(UserFaceColor1 == Corner.FaceColor2) FaceNo1 = Corner.FaceNo2;
			else if(UserFaceColor1 == Corner.FaceColor3) FaceNo1 = Corner.FaceNo3;
			else continue;
				
			// match color2
			int FaceNo2;
			if(UserFaceColor2 == Corner.FaceColor1) FaceNo2 = Corner.FaceNo1;
			else if(UserFaceColor2 == Corner.FaceColor2) FaceNo2 = Corner.FaceNo2;
			else if(UserFaceColor2 == Corner.FaceColor3) FaceNo2 = Corner.FaceNo3;
			else continue;

			// match color3
			int FaceNo3;
			if(UserFaceColor3 == Corner.FaceColor1) FaceNo3 = Corner.FaceNo1;
			else if(UserFaceColor3 == Corner.FaceColor2) FaceNo3 = Corner.FaceNo2;
			else if(UserFaceColor3 == Corner.FaceColor3) FaceNo3 = Corner.FaceNo3;
			else continue;

			// save the three face numbers in user face aaary
			UserFaceArray[StandardFaceNo1] = FaceNo1;
			UserFaceArray[StandardFaceNo2] = FaceNo2;
			UserFaceArray[StandardFaceNo3] = FaceNo3;
			return;
			}

		throw new ApplicationException(string.Format("Corner faces colors {0}, {1} and {2} are in error",
				FaceColorName[UserFaceColor1], FaceColorName[UserFaceColor2], FaceColorName[UserFaceColor3]));
		}

	/// <summary>
	/// Test edges
	/// </summary>
	/// <param name="EdgeBlock">Standard edge block</param>
	/// <param name="UserColorArray">User color array</param>
	/// <param name="UserFaceArray">User face array</param>
	private static void TestUserEdge
			(
			EdgeBlock EdgeBlock,
			int[] UserColorArray,
			int[] UserFaceArray
			)
		{
		// two face numbers assoicated with the edge
		int StandardFaceNo1 = EdgeBlock.FaceNo1;
		int StandardFaceNo2 = EdgeBlock.FaceNo2;

		// two face colors after the user paint the faces
		int FaceColor1 = UserColorArray[StandardFaceNo1];
		int FaceColor2 = UserColorArray[StandardFaceNo2];

		// make sure all colors are different
		if(FaceColor1 == FaceColor2)
			throw new ApplicationException(string.Format("Edge faces colors {0} and {1} must be different.",
				FaceColorName[FaceColor1], FaceColorName[FaceColor2]));

		// look for the edge in the standard solved cube
		foreach(EdgeBlock Edge in EdgeBlockArray)
			{
			// match color 1
			int FaceNo1;
			if(FaceColor1 == Edge.FaceColor1) FaceNo1 = Edge.FaceNo1;
			else if(FaceColor1 == Edge.FaceColor2) FaceNo1 = Edge.FaceNo2;
			else continue;
			
			// match color 2
			int FaceNo2;
			if(FaceColor2 == Edge.FaceColor1) FaceNo2 = Edge.FaceNo1;
			else if(FaceColor2 == Edge.FaceColor2) FaceNo2 = Edge.FaceNo2;
			else continue;

			// save the two face numbers in user face aaary
			UserFaceArray[StandardFaceNo1] = FaceNo1;
			UserFaceArray[StandardFaceNo2] = FaceNo2;
			return;
			}

		throw new ApplicationException(string.Format("Edge faces colors {0} and {1} are in error",
				FaceColorName[FaceColor1], FaceColorName[FaceColor2]));
		}

	/// <summary>
	/// Creat random rotation steps array
	/// </summary>
	/// <param name="Moves">Number of rotation steps</param>
	/// <param name="LastMove">Previous move or -1 for none</param>
	/// <returns>Steps Array</returns>
	public static int[] Scramble
			(
			int Moves,
			int LastMove
			)
		{
		// output array
		int[] Steps = new int[Moves];

		// perform random moves
		for(int Index = 0; Index < Moves;)
			{
			// generate 0 to 17 random integer
			int Move = RandomMove.Next(RotationCodes);

			// ignore the move if previous move was the same side
			if(LastMove >= 0 && Move / RotMovesPerColor == LastMove / RotMovesPerColor) continue;

			// save move
			Steps[Index++] = Move;
			LastMove = Move;
			}
		return Steps;
		}

	/// <summary>
	/// Convert color steps to text message
	/// </summary>
	/// <param name="Steps">Color steps array</param>
	/// <returns>Message</returns>
	public static string ColorCodesToText
			(
			int[] Steps
			)
		{
		StringBuilder Text = new StringBuilder();
		for(int Index = 0; Index < Steps.Length; Index++)
			{
			string Separator;
			if(Index == 0) Separator = string.Empty;
			else if((Index % 3) == 0) Separator = " - ";
			else Separator = " ";
			Text.AppendFormat("{0}{1}", Separator, ColorRotationName[Steps[Index]]);
			}
		return Text.ToString();
		}

	/// <summary>
	/// Convert relative steps to text message
	/// </summary>
	/// <param name="UpFaceColor">Up face color</param>
	/// <param name="FrontFaceColor">Front face color</param>
	/// <param name="Steps">Relative steps array</param>
	/// <returns>Message</returns>
	public static string RelativeCodesToText
			(
			int UpFaceColor,
			int FrontFaceColor,
			int[] Steps
			)
		{
		StringBuilder Text = new StringBuilder();

		// convert color steps to relative steps
		int[] Xlate = ColorToRelative[(FrontFaceColor - 1) + (UpFaceColor == WhiteFace ? 0 : 4)];
		for(int Index = 0; Index < Steps.Length; Index++)
			{
			int Step = Steps[Index];
			string Separator;
			if(Index == 0) Separator = string.Empty;
			else if((Index % 3) == 0) Separator = " - ";
			else Separator = " ";
			Text.AppendFormat("{0}{1}", Separator, RelativeRotationName[Xlate[Step / 3] + (Step % 3)]);
			}
		return Text.ToString();
		}
	}
}
