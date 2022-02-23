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
	/// Wrapper class to save block face
	/// </summary>
	public class ModelVisual3DCube : ModelVisual3D
		{
		/// <summary>
		/// Block face
		/// </summary>
		public BlockFace3D BlockFace;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="BlockFace">Block face</param>
		/// <param name="GeometryModel">Geometry model</param>
		public ModelVisual3DCube
				(
				BlockFace3D BlockFace,
				GeometryModel3D GeometryModel
				)
			{
			this.BlockFace = BlockFace;
			Content = GeometryModel;
			return;
			}
		}
	}
