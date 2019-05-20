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

using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace UziRubiksCube
{
/// <summary>
/// Block face
/// </summary>
public class BlockFace3D
	{
	/// <summary>
	/// Block face number. Movable block 0-47, Fixed block 48 52, Invisible block -1
	/// </summary>
	public int FaceNo;

	/// <summary>
	/// Current block face number
	/// </summary>
	public int CurrentColor;

	private GeometryModel3D TrigGeometry1;
	private GeometryModel3D TrigGeometry2;

	/// <summary>
	/// Block face constructor
	/// </summary>
	/// <param name="Block">Parent Block</param>
	/// <param name="FaceNo">Face number</param>
	/// <param name="FaceColor">Initial face color</param>
	public BlockFace3D
			(
			Block3D Block,
			int FaceNo,
			int FaceColor
			)
		{
		// save face number
		this.FaceNo = FaceNo;

		// set current color
		CurrentColor = FaceColor;

		// initialize some geometric variables
		Point3D Point0 = new Point3D();
		Point3D Point1 = new Point3D();
		Point3D Point2 = new Point3D();
		Point3D Point3 = new Point3D();
		Vector3D Normal = new Vector3D();

		switch(FaceColor)
			{
			case Cube.WhiteFace:
				Point0 = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ);
				Point1 = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
				Point2 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
				Point3 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ);
				Normal = new Vector3D(0, 0, -1);
				break;

			case Cube.BlueFace:
				Point0 = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
				Point1 = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
				Point2 = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ);
				Point3 = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
				Normal = new Vector3D(-1, 0, 0);
				break;

			case Cube.RedFace:
				Point0 = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
				Point1 = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ);
				Point2 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ);
				Point3 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
				Normal = new Vector3D(0, -1, 0);
				break;

			case Cube.GreenFace:
				Point0 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
				Point1 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ);
				Point2 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
				Point3 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
				Normal = new Vector3D(1, 0, 0);
				break;

			case Cube.OrangeFace:
				Point0 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
				Point1 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
				Point2 = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ);
				Point3 = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
				Normal = new Vector3D(0, 1, 0);
				break;

			case Cube.YellowFace:
				Point0 = new Point3D(Block.OrigX, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
				Point1 = new Point3D(Block.OrigX, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
				Point2 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY, Block.OrigZ + Cube3D.BlockWidth);
				Point3 = new Point3D(Block.OrigX + Cube3D.BlockWidth, Block.OrigY + Cube3D.BlockWidth, Block.OrigZ + Cube3D.BlockWidth);
				Normal = new Vector3D(0, 0, 1);
				break;
			}

		// hidden black faces
		if(FaceNo < 0)
			{
			DiffuseMaterial BlackMaterial = new DiffuseMaterial(Brushes.Black);
			Block.Children.Add(CreateTriangle(Point0, Point1, Point2, Normal, BlackMaterial));
			Block.Children.Add(CreateTriangle(Point0, Point2, Point3, Normal, BlackMaterial));
			return;
			}

		// calculate points to separate block edge and block face
		Vector3D Diag02 = Point3D.Subtract(Point2, Point0);
		Point3D Point4 = Point3D.Add(Point0, Vector3D.Multiply(0.04, Diag02)); 
		Point3D Point6 = Point3D.Add(Point0, Vector3D.Multiply(0.96, Diag02)); 

		Vector3D Diag13 = Point3D.Subtract(Point3, Point1);
		Point3D Point5 = Point3D.Add(Point1, Vector3D.Multiply(0.04, Diag13)); 
		Point3D Point7 = Point3D.Add(Point1, Vector3D.Multiply(0.96, Diag13)); 

		// gray edge		
		DiffuseMaterial GrayMaterial = new DiffuseMaterial(Brushes.DarkGray);
		Block.Children.Add(CreateTriangle(Point0, Point1, Point5, Normal, GrayMaterial));
		Block.Children.Add(CreateTriangle(Point0, Point5, Point4, Normal, GrayMaterial));

		Block.Children.Add(CreateTriangle(Point1, Point2, Point6, Normal, GrayMaterial));
		Block.Children.Add(CreateTriangle(Point1, Point6, Point5, Normal, GrayMaterial));

		Block.Children.Add(CreateTriangle(Point2, Point3, Point7, Normal, GrayMaterial));
		Block.Children.Add(CreateTriangle(Point2, Point7, Point6, Normal, GrayMaterial));

		Block.Children.Add(CreateTriangle(Point3, Point0, Point4, Normal, GrayMaterial));
		Block.Children.Add(CreateTriangle(Point3, Point4, Point7, Normal, GrayMaterial));

		// block face color
		DiffuseMaterial ColorMaterial = Cube3D.Material[FaceColor];
		Block.Children.Add(CreateTriangle(Point4, Point5, Point6, Normal, ColorMaterial, 1));
		Block.Children.Add(CreateTriangle(Point4, Point6, Point7, Normal, ColorMaterial, 2));
		return;
		}

	/// <summary>
	/// Create one triangle
	/// </summary>
	/// <param name="Point0">Pont 0</param>
	/// <param name="Point1">Point 1</param>
	/// <param name="Point2">Point 2</param>
	/// <param name="Normal">Normal vector toward outside the cube</param>
	/// <param name="Material">Color of the triangle</param>
	/// <param name="ColorTriangle">Face color or gray frame color</param>
	/// <returns>Model visual 3D</returns>
	private ModelVisual3D CreateTriangle
			(
			Point3D Point0,
			Point3D Point1,
			Point3D Point2,
			Vector3D Normal,
			DiffuseMaterial Material,
			int ColorTriangle = 0
			)
		{
		MeshGeometry3D TriangleMesh = new MeshGeometry3D();
		TriangleMesh.Positions.Add(Point0);
		TriangleMesh.Positions.Add(Point1);
		TriangleMesh.Positions.Add(Point2);

		TriangleMesh.TriangleIndices.Add(0);
		TriangleMesh.TriangleIndices.Add(1);
		TriangleMesh.TriangleIndices.Add(2);

		TriangleMesh.Normals.Add(Normal);
		TriangleMesh.Normals.Add(Normal);
		TriangleMesh.Normals.Add(Normal);

		GeometryModel3D GeometryModel = new GeometryModel3D(TriangleMesh, Material);
		if(ColorTriangle == 1) TrigGeometry1 = GeometryModel;
		else if(ColorTriangle == 2) TrigGeometry2 = GeometryModel;
		return new ModelVisual3DCube(this, GeometryModel);
		}

	/// <summary>
	/// Change the color of this face
	/// </summary>
	/// <param name="NewColor">New color</param>
	public void ChangeColor
			(
			int NewColor
			)
		{
		CurrentColor = NewColor;
		DiffuseMaterial Material = Cube3D.Material[NewColor];
		TrigGeometry1.Material = Material;
		TrigGeometry2.Material = Material;
		return;
		}
	}
}
