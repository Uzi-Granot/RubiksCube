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

using System.Windows.Media.Media3D;

namespace UziRubiksCube
	{
	/// <summary>
	/// Rubik's cube block class
	/// </summary>
	public class Block3D : ModelVisual3D
		{
		/// <summary>
		/// Array of 6 block faces
		/// </summary>
		public BlockFace3D[] BlockFaceArray;

		/// <summary>
		/// Block X position
		/// </summary>
		public double OrigX;

		/// <summary>
		/// Block Y position
		/// </summary>
		public double OrigY;

		/// <summary>
		/// Block Z position
		/// </summary>
		public double OrigZ;

		/// <summary>
		/// Block constructor
		/// </summary>
		/// <param name="BlockNo">Block number (0 to 26)</param>
		public Block3D
				(
				int BlockNo
				)
			{
			// for the hidden center block there is no more initialization
			if (BlockNo == 13) return;

			// block origin
			OrigX = -Cube3D.HalfCubeWidth + (BlockNo % 3) * (Cube3D.BlockWidth + Cube3D.BlockSpacing);
			OrigY = -Cube3D.HalfCubeWidth + ((BlockNo / 3) % 3) * (Cube3D.BlockWidth + Cube3D.BlockSpacing);
			OrigZ = -Cube3D.HalfCubeWidth + (BlockNo / 9) * (Cube3D.BlockWidth + Cube3D.BlockSpacing);

			// array of the 6 faces of this block
			BlockFaceArray = new BlockFace3D[6];

			// loop for all 6 faces
			for (int FaceColor = 0; FaceColor < Cube.FaceColors; FaceColor++)
				{
				// create 6 faces for each block
				// translate block number and face to face number
				BlockFaceArray[FaceColor] = new BlockFace3D(this, Cube.BlockFace[BlockNo, FaceColor], FaceColor);
				}
			return;
			}
		}
	}
