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
//	Copyright (C) 2017-2022 Uzi Granot. All Rights Reserved
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
//	For version history please refer to MainWindow.xaml.cs
/////////////////////////////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace UziRubiksCube
	{
	public class Cube3D : ModelVisual3D
		{
		// constats that can be modified for appearance

		/// <summary>
		/// Single block width height and depth
		/// </summary>
		public const double BlockWidth = 1.0;

		/// <summary>
		/// Spacing between blocks
		/// </summary>
		public const double BlockSpacing = 0.05;

		/// <summary>
		/// Cube width height and depth
		/// </summary>
		public const double CubeWidth = 3 * BlockWidth + 2 * BlockSpacing;

		/// <summary>
		/// Half cube width
		/// </summary>
		public const double HalfCubeWidth = 0.5 * CubeWidth;

		/// <summary>
		/// Camera distance from cube
		/// </summary>
		public const double CameraDistance = 4.0 * CubeWidth;

		/// <summary>
		/// Camera angle above horizontal plane 
		/// </summary>
		public const double CameraUpAngle = 25.0 * Math.PI / 180.0;

		/// <summary>
		/// Camera angle to the right
		/// </summary>
		public const double CameraRightAngle = 25.0 * Math.PI / 180.0;

		/// <summary>
		/// Camera view angle in degrees
		/// </summary>
		public const double CameraViewAngle = 40;

		/// <summary>
		/// Thin border
		/// </summary>
		internal static Thickness ThinBorder = new(1);

		/// <summary>
		/// Thick border
		/// </summary>
		internal static Thickness ThickBorder = new(5);

		/// <summary>
		/// Face color array
		/// </summary>
		internal static Brush[] FaceColor = new Brush[]
			{
			Brushes.White,
			Brushes.Blue,
			Brushes.Red,
			Brushes.Green,
			Brushes.Orange,
			Brushes.Yellow,
			Brushes.Black,
			};

		/// <summary>
		/// Block face diffuse maretial
		/// </summary>
		internal static DiffuseMaterial[] Material = new DiffuseMaterial[]
			{
			new DiffuseMaterial(Brushes.White),
			new DiffuseMaterial(Brushes.Blue),
			new DiffuseMaterial(Brushes.Red),
			new DiffuseMaterial(Brushes.Green),
			new DiffuseMaterial(Brushes.Orange),
			new DiffuseMaterial(Brushes.Yellow),
			new DiffuseMaterial(Brushes.Black)
			};

		/// <summary>
		/// Cube's faces Rotation axis
		/// </summary>
		internal static Vector3D[] RotationAxis = new Vector3D[]
			{
			new Vector3D(0, 0, 1),
			new Vector3D(1, 0, 0),
			new Vector3D(0, 1, 0),
			new Vector3D(-1, 0, 0),
			new Vector3D(0, -1, 0),
			new Vector3D(0, 0, -1),
			};

		/// <summary>
		/// Rotation moves names
		/// </summary>
		internal static string[] RotMoveName = new string[]
			{
			"CW",
			"CW2",
			"CCW",
			};

		/// <summary>
		/// full cube motion
		/// </summary>
		internal static int[][] FullMoveAngle = new int[][]
			{
			new int[] {-90, 0, 0},		// white
			new int[] {0, 0, 90},		// blue
			new int[] {0, 0, 0},		// red
			new int[] {0, 0, -90},		// green
			new int[] {0, 0, 180},		// orange
			new int[] {90, 0, 0},       // yellow
			};

		/// <summary>
		/// Cube sides animation rotation angles (no rotation, CW, CW2, and CCW
		/// </summary>
		internal static int[] RotMoveAngle = new int[]
			{
			0,
			90,
			180,
			-90,
			};

		/// <summary>
		/// for given top color what are the available colors for front face
		/// </summary>
		internal static int[][] FullMoveTopColor = new int[][]
			{
			new int[] {2, 3, 4, 1},		// white
			new int[] {5, 2, 0, 4},		// blue
			new int[] {5, 3, 0, 1},		// red
			new int[] {5, 4, 0, 2},		// green
			new int[] {5, 1, 0, 3},		// orange
			new int[] {4, 3, 2, 1},     // yellow
			};

		/// <summary>
		/// block number by color code and block position on the cube face
		/// </summary>
		internal static int[,] BlockNoOfOneFace = new int[,]
			{
			{  0,  1,  2,  3,  4,  5,  6,  7,  8},		// white
			{  0,  3,  6,  9, 12, 15, 18, 21, 24},		// blue
			{  0,  1,  2,  9, 10, 11, 18, 19, 20},		// red
			{  2,  5,  8, 11, 14, 17, 20, 23, 26},		// green
			{  6,  7,  8, 15, 16, 17, 24, 25, 26},		// orange
			{ 18, 19, 20, 21, 22, 23, 24, 25, 26},      // yellow
			};

		/// <summary>
		/// Cube object
		/// </summary>
		public Cube FullCube;

		/// <summary>
		/// All the blocks of each side of the cube
		/// </summary>
		public Block3D[][] CubeFaceBlockArray;

		/// <summary>
		/// Animaton block face array of one cube side
		/// </summary>
		public BlockFace3D[] MovableFaceArray;

		/// <summary>
		/// Constructor
		/// </summary>
		public Cube3D()
			{
			// initialize cube object
			FullCube = new Cube();

			MovableFaceArray = new BlockFace3D[Cube.MovableFaces];
			for (int BlockNo = 0; BlockNo < Cube.BlocksPerCube; BlockNo++)
				{
				// create block
				Block3D Block = new(BlockNo);

				// add box to Cube3D VisualMedia3D
				Children.Add(Block);

				// center block (block no 13)
				if (Block.BlockFaceArray == null) continue;

				// loop forall 6 faces of each block
				foreach (BlockFace3D Face in Block.BlockFaceArray)
					{
					// save movable faces array
					if (Face.FaceNo >= 0 && Face.FaceNo < Cube.MovableFaces) MovableFaceArray[Face.FaceNo] = Face;
					}
				}

			// create double index array of all 6 faces and for each face all blocks
			CubeFaceBlockArray = new Block3D[Cube.FaceColors][];

			// loop for all colors
			for (int ColorIndex = 0; ColorIndex < Cube.FaceColors; ColorIndex++)
				{
				// for each color there are 9 blocks
				CubeFaceBlockArray[ColorIndex] = new Block3D[Cube.BlocksPerFace];

				// loop for all blocks of one face color
				for (int BlockIndex = 0; BlockIndex < Cube.BlocksPerFace; BlockIndex++)
					{
					CubeFaceBlockArray[ColorIndex][BlockIndex] = (Block3D)Children[BlockNoOfOneFace[ColorIndex, BlockIndex]];
					}
				}
			return;
			}

		/// <summary>
		/// Set the color of all faces to agree with Cube.FaceArray
		/// </summary>
		public void SetColorOfAllFaces()
			{
			for (int FaceNo = 0; FaceNo < Cube.MovableFaces; FaceNo++)
				{
				// change color of face no with the color of face pos
				int FaceColor = FullCube.FaceColor(FaceNo);
				if (MovableFaceArray[FaceNo].CurrentColor != FaceColor) MovableFaceArray[FaceNo].ChangeColor(FaceColor);
				}
			return;
			}
		}
	}
