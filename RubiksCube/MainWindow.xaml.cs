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
//	Version History:
//
//	Version 1.0 2017/08/01
//		Original revision
//	Version 1.0.1 2017/08/01
//		Fix debug working directory
//	Version 2.0.0 2022/02/20
//		Upgrade to VS2022 and .NET6
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace UziRubiksCube
	{
	/// <summary>
	/// Rotation type active
	/// </summary>
	public enum RotationActive
		{
		Idle,
		Cube,	// whole cube rotation
		Face,	// one face rotation
		}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
	{
	private const double ButtonsPanelWidth = 280;
	private const double InfoPanelHeight = 100;

	private Viewport3D CubeViewPort3D;
	private Cube3D RubiksCube3D;
	private Point LastMousePosition;
	private AxisAngleRotation3D FullCubeXRotation;
	private AxisAngleRotation3D FullCubeYRotation;
	private AxisAngleRotation3D FullCubeZRotation;
	private Transform3DGroup RotateTransformGroup;

	private int FrontFaceColor;
	private int TopFaceColor;
	private Button[] FrontFaceButtons;
	private Button[] UpFaceButtons;

	private bool UserCubeActive;
	private int UserCubeSelection;
	private Button[] UserColorButtons;
	public int[] UserColorArray;

	private RotationActive RotationActive;
	private List<BlockFace3D> HitList;
	private bool RotationLock;
	private int RotateCode;
	private List<int> PastMoves;
	private List<int> NextMoves;
	private bool AutoSolve;

	private static readonly DoubleAnimation[] AnimationArray =
		{
		new DoubleAnimation(90.0, new Duration(new TimeSpan(2500000))),
		new DoubleAnimation(180.0, new Duration(new TimeSpan(5000000))),
		new DoubleAnimation(-90.0, new Duration(new TimeSpan(2500000))),
		};

	/// <summary>
	/// Main window constructor
	/// </summary>
	public MainWindow()
		{
		InitializeComponent();
		Title="Rubik's Cube for beginners (Rev 2.0.0 Date 2022-02-20)";
		return;
		}

	/// <summary>
	/// Application initialization
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void OnMainGridLoaded(object sender, RoutedEventArgs e)
		{
		// debug mode
		#if DEBUG
		// change current directory to work directory if exist
		string CurDir = Environment.CurrentDirectory;
		int Index = CurDir.IndexOf("bin\\Debug");
		if (Index > 0)
			{
			string WorkDir = string.Concat(CurDir.AsSpan(0, Index), "Work");
			if (Directory.Exists(WorkDir)) Environment.CurrentDirectory = WorkDir;
			}

		// open trace file
		Trace.Open("RubiksCubeTrae.txt");
		#endif

		// top face color buttons
		UpFaceButtons = new Button[Cube.FaceColors];
		UpFaceButtons[0] = UpFaceButton0;
		UpFaceButtons[1] = UpFaceButton1;
		UpFaceButtons[2] = UpFaceButton2;
		UpFaceButtons[3] = UpFaceButton3;
		UpFaceButtons[4] = UpFaceButton4;
		UpFaceButtons[5] = UpFaceButton5;

		// front face color buttons
		FrontFaceButtons = new Button[Cube.FaceColors];
		FrontFaceButtons[0] = FrontFaceButton0;
		FrontFaceButtons[1] = FrontFaceButton1;
		FrontFaceButtons[2] = FrontFaceButton2;
		FrontFaceButtons[3] = FrontFaceButton3;
		FrontFaceButtons[4] = FrontFaceButton4;
		FrontFaceButtons[5] = FrontFaceButton5;

		// user cube color buttons
		UserColorButtons = new Button[Cube.FaceColors];
		UserColorButtons[0] = SetColorButton0;
		UserColorButtons[1] = SetColorButton1;
		UserColorButtons[2] = SetColorButton2;
		UserColorButtons[3] = SetColorButton3;
		UserColorButtons[4] = SetColorButton4;
		UserColorButtons[5] = SetColorButton5;

		// set animation complete method
		AnimationArray[0].Completed += AnimationCompleted;
		AnimationArray[1].Completed += AnimationCompleted;
		AnimationArray[2].Completed += AnimationCompleted;
		return;
		}

	/// <summary>
	/// Cube Grid initialization
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void OnCubeGridLoaded(object sender, RoutedEventArgs e)
		{
		Reset();
		return;
		}

	/// <summary>
	/// Reset button pressed
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void ResetButtonClick(object sender, RoutedEventArgs e)
		{
		Reset();
		return;
		}

	/// <summary>
	/// Reset to startup conditions
	/// </summary>
	private void Reset()
		{
		// trace file
		#if DEBUG
		Trace.Write("Reset");
		#endif

		// create Viewport3D and add it to CubeGrid parent
		CubeViewPort3D = new Viewport3D()
			{
			//Name = "mainViewport",
			ClipToBounds = true
			};
		CubeGrid.Children.Clear();
		CubeGrid.Children.Add(CubeViewPort3D);

		// create ModelVisual3D and add it to Viewport3D
		ModelVisual3D ModelVisual = new();
		CubeViewPort3D.Children.Add(ModelVisual);

		// create Model3DGroup with white ambiant light and attach it to ModelViual
		Model3DGroup ModelGroup = new();
		ModelGroup.Children.Add(new AmbientLight(Colors.White));
		ModelVisual.Content = ModelGroup;

		// create our rubik's cube and attach it to ViewPort
		RubiksCube3D = new Cube3D();
		CubeViewPort3D.Children.Add(RubiksCube3D);

		// camera position relative to the cube
		// camera is looking directly to the cube
		double PosZ = Cube3D.CameraDistance * Math.Sin(Cube3D.CameraUpAngle);
		double Temp = Cube3D.CameraDistance * Math.Cos(Cube3D.CameraUpAngle);
		double PosX = Temp * Math.Sin(Cube3D.CameraRightAngle);
		double PosY = Temp * Math.Cos(Cube3D.CameraRightAngle);

		// create camera and attach it to ViewPort
		CubeViewPort3D.Camera = new PerspectiveCamera(new Point3D(PosX, -PosY, PosZ), 
			new Vector3D(-PosX, PosY, -PosZ), new Vector3D(0, 0, 1), Cube3D.CameraViewAngle);

		// full cube motion transformation group allows the program to
		// rotate the whole cube in any direction
		Transform3DGroup FullCubeMotion = new();
		RubiksCube3D.Transform = FullCubeMotion;
		FullCubeZRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
		FullCubeMotion.Children.Add(new RotateTransform3D(FullCubeZRotation));
		FullCubeXRotation = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0);
		FullCubeMotion.Children.Add(new RotateTransform3D(FullCubeXRotation));
		FullCubeYRotation = new AxisAngleRotation3D(new Vector3D(0, 1, 0), 0);
		FullCubeMotion.Children.Add(new RotateTransform3D(FullCubeYRotation));

		// list of active solution steps
		NextMoves = new List<int>();

		// list of saved moves to be used by undo button
		PastMoves = new List<int>();

		// type of rotation that is active
		RotationActive = RotationActive.Idle;

		// mouse left button hit list
		HitList = new List<BlockFace3D>();
		RotationLock = false;

		// current top and front face color
		TopFaceColor = -1;
		FrontFaceColor = -1;
		SetUpAndFrontFace(true, 2, 0);

		// user is setting the cube
		UserCubeActive = false;
		UserCubeButton.Content = "User Cube";
		UserCubeSelection = 0;
		SetColorButtonClick(null, null);

		// clear information labels
		ResetInfoLabels(false);
		return;
		}

	/// <summary>
	/// Rotation button was pressed 
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void RotationButtonClick(object sender, RoutedEventArgs e)
		{
		if(!RotationLock && !UserCubeActive) RotateSide(TagTranslator(sender));
		return;
		}

	/// <summary>
	/// Front face button was pressed
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void FrontFaceButtonClick(object sender, RoutedEventArgs e)
		{
		if(!RotationLock) SetUpAndFrontFace(false, TagTranslator(sender), TopFaceColor);
		return;
		}

	/// <summary>
	/// Top face button was pressed
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void UpFaceButtonClick(object sender, RoutedEventArgs e)
		{
		if(!RotationLock) SetUpAndFrontFace(true, FrontFaceColor, TagTranslator(sender));
		return;
		}

	/// <summary>
	/// Set front or top face color
	/// </summary>
	/// <param name="TopFaceClick">Top face if true, front face if false</param>
	/// <param name="FrontFaceColor">Front face color</param>
	/// <param name="TopFaceColor">Top face color</param>
	private void SetUpAndFrontFace
			(
			bool TopFaceClick,
			int FrontFaceColor,
			int TopFaceColor
			)
		{
		// no change
		if(FrontFaceColor == this.FrontFaceColor && TopFaceColor == this.TopFaceColor) return;

		// front face
		if(!TopFaceClick)
			{
			int TopIndex;
			for(TopIndex = 0; TopIndex < 4; TopIndex++)
				{
				if(Cube3D.FullMoveTopColor[FrontFaceColor][TopIndex] == TopFaceColor) break;
				}
			if(TopIndex == 4) TopFaceColor = Cube3D.FullMoveTopColor[FrontFaceColor][0];
			}

		// top face
		else
			{
			int FrontIndex;
			for(FrontIndex = 0; FrontIndex < 4; FrontIndex++)
				{
				if(Cube3D.FullMoveTopColor[TopFaceColor][FrontIndex] == FrontFaceColor) break;
				}
			if(FrontIndex == 4) FrontFaceColor = Cube3D.FullMoveTopColor[TopFaceColor][0];
			}

		// save
		this.FrontFaceColor = FrontFaceColor;
		this.TopFaceColor = TopFaceColor;

		// set thick border
		for(int Index = 0; Index < Cube.FaceColors; Index++)
			{
			UpFaceButtons[Index].BorderThickness = Index == TopFaceColor ? Cube3D.ThickBorder : Cube3D.ThinBorder;
			FrontFaceButtons[Index].BorderThickness = Index == FrontFaceColor ? Cube3D.ThickBorder : Cube3D.ThinBorder;
			}		

		// y axis rotation angle
		int YIndex;
		for(YIndex = 0; YIndex < 4; YIndex++)
			{
			if(Cube3D.FullMoveTopColor[FrontFaceColor][YIndex] == TopFaceColor) break;
			}

		// rotate the full cube
		FullCubeXRotation.Angle = Cube3D.FullMoveAngle[FrontFaceColor][0];
		FullCubeYRotation.Angle = -Cube3D.RotMoveAngle[YIndex];
		FullCubeZRotation.Angle = Cube3D.FullMoveAngle[FrontFaceColor][2];
		return;
		}

	/// <summary>
	/// Undo button clicked
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void UndoButtonClick(object sender, RoutedEventArgs e)
		{
		if(!RotationLock && !UserCubeActive && PastMoves.Count != 0) RotateSide(-1);
		return;
		}

	/// <summary>
	/// Rotate one side using the mouse
	/// </summary>
	private void RotateByMouse()
		{
		// we need two hits
		if(HitList.Count != 2) return;

		// get from and to face numbers
		int FromFaceNo = HitList[0].FaceNo;
		int ToFaceNo = HitList[1].FaceNo;
		if(FromFaceNo == ToFaceNo) return;

		// both faces must be on the same side
		if(FromFaceNo / 8 != ToFaceNo / 8) return;

		// both faces must be either edges or corners
		if((FromFaceNo & 1) != (ToFaceNo & 1)) return;

		// calculate move size 1=CW 2=CW2 3=CCW
		int FromEdgeNo = (FromFaceNo % 8) / 2;
		int ToEdgeNo = (ToFaceNo % 8) / 2;
		int Delta = ToEdgeNo - FromEdgeNo;
		if(Delta < 0) Delta += 4;

		// convert to rotation code
		RotateSide(3 * (FromFaceNo / 8) + (Delta - 1));
		return;
		}

	/// <summary>
	/// Rotate one side
	/// </summary>
	/// <param name="RotateCode">Rotation code</param>
	private void RotateSide
			(
			int RotateCode
			)
		{
		// rotate code
		if(RotateCode >= 0)
			{
			PastMoves.Add(RotateCode);
			}
		// undo-take rotation code from past move list
		else
			{
			// trace
			#if DEBUG
			Trace.Write("Undo");
			#endif

			RotateCode = PastMoves[^1];
			PastMoves.RemoveAt(PastMoves.Count - 1);
			switch(RotateCode % 3)
				{
				case 0:
					RotateCode += 2;
					break;

				case 2:
					RotateCode -= 2;
					break;
				}
			}

		// save rotation code
		this.RotateCode = RotateCode;

		// there are 18 rotation codes 3 for each cube face
		// RotateFace 0=white, 1=blue, 2=red, 3=green, 4=orange, 5=yellow
		int RotateFace = RotateCode / 3;

		// create a transform group
		RotateTransformGroup = new Transform3DGroup();

		// attach the transform group object to all 9 blocks of the face to be rotated
		for(int Index = 0; Index < Cube.BlocksPerFace; Index++)
			{
			RubiksCube3D.CubeFaceBlockArray[RotateFace][Index].Transform = RotateTransformGroup;
			}

		// create axis rotation for the transform group
		AxisAngleRotation3D AxisRot = new(Cube3D.RotationAxis[RotateFace], 0);
		RotateTransformGroup.Children.Add(new RotateTransform3D(AxisRot));

		// start the animation
		// note: AnimationArray is an array of 3 DoubleAnnimation objects.
		// The three objects are 90Deg 0.25Sec, 180Deg 0.5Sec, -90Deg 0.25Sec
		AxisRot.BeginAnimation(AxisAngleRotation3D.AngleProperty, AnimationArray[RotateCode % 3]);

		// set rotation lock
		RotationLock = true;
		return;
		}

	/// <summary>
	/// Animation completed event
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void AnimationCompleted(object sender, EventArgs e)
		{
		// clear all block children of the side that was rotated
		RotateTransformGroup.Children.Clear();

		// rotate the current face array of the cube object
		RubiksCube3D.FullCube.RotateArray(RotateCode);

		// set the color of all block faces of the cube
		RubiksCube3D.SetColorOfAllFaces();

		// reset the transform fields of each face that was part of the group
		for(int Index = 0; Index < Cube.BlocksPerFace; Index++)
			RubiksCube3D.CubeFaceBlockArray[RotateCode / 3][Index].Transform = null;

		// the cube is in perfect order
		if(RubiksCube3D.FullCube.AllBlocksInPlace)
			{
			// clear next move list
			NextMoves.Clear();

			// reset auto solve flag
			AutoSolve = false;
			}

		// cube is not in solution order and next moves list is not empty
		else if(NextMoves.Count != 0)
			{
			// perform next move and remove it from the list
			int NextMove = NextMoves[0];
			NextMoves.RemoveAt(0);
			RotateSide(NextMove);
			}

		// cube is not in order and auto solve is on
		else if(AutoSolve)
			{
			// solve next step
			SolveCube();
			}

		// clear rotation lock
		RotationLock = false;
		return;
		}

	/// <summary>
	/// User pressed on mouse's left button
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
		// ignore if rotation lock is on
		if(RotationLock) return;

		// mouse current position
		Point MousePosition = e.GetPosition(CubeViewPort3D);

		// user is not setting colors
		if(!UserCubeActive)
			{
			// clear hit list if rotation is idle or full cube
			if(RotationActive != RotationActive.Face) HitList.Clear();

			// hit test for current position
			HitTest(MousePosition);

			// hit list is empty
			if(HitList.Count == 0)
				{
				// save position
				LastMousePosition = MousePosition;

				// set rotation active to full cube rotation
				Cursor = Cursors.SizeAll;
				RotationActive = RotationActive.Cube;
				}

			// there is one entry in hit list
			else if(HitList.Count == 1 && RotationActive != RotationActive.Face)
				{
				// set rotation active to one face rotation
				RotationActive = RotationActive.Face;
				Cursor = Cursors.Hand;
				}

			// rotate one face by mouse action
			else
				{
				RotationActive = RotationActive.Idle;
				Cursor = Cursors.Arrow;
				RotateByMouse();
				}
			}

		// user is painting the cube to agree with his/her cube
		else
			{
			HitList.Clear();
			HitTest(MousePosition);
			if(HitList.Count > 0)
				{
				int FaceNo = HitList[^1].FaceNo;
				if(FaceNo >= 0 && FaceNo < Cube.MovableFaces)
					{
					// save color
					UserColorArray[FaceNo] = UserCubeSelection;

					// change color of block face
					RubiksCube3D.MovableFaceArray[FaceNo].ChangeColor(UserCubeSelection);
					}
				}
			}
		return;  
		}

	/// <summary>
	/// Left button up
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
 	private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
		// if we are in rotate full cube, reset rotation
		if(RotationActive == RotationActive.Cube)
			{
			RotationActive = RotationActive.Idle;
			Cursor = Cursors.Arrow;
			}
		return; 
		}

	/// <summary>
	/// Mouse pointer is moving
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void OnMouseMove(object sender, MouseEventArgs e)
		{
		// if we are in full cube rotation mode, rotate the cube
		if(RotationActive == RotationActive.Cube) FullCubeRotation(e.GetPosition(CubeViewPort3D));
		return;
		}
 
	/// <summary>
	/// Full cube rotation
	/// </summary>
	/// <param name="MousePosition">Mouse position</param>
	private void FullCubeRotation
			(
			Point MousePosition
			)
		{
		const int Step = 3;

		// change in mouse position
		double DeltaX = MousePosition.X - LastMousePosition.X;
		double DeltaY = MousePosition.Y - LastMousePosition.Y;

		// save for next step
		LastMousePosition = MousePosition;

		// positive x change
		if(DeltaX > 0)
			{
			double Angle = FullCubeZRotation.Angle + Step;
			if(Angle > 180) Angle -= 360;
			FullCubeZRotation.Angle = Angle;
			}

		// negative x change
		else if(DeltaX < 0)
			{
			double Angle = FullCubeZRotation.Angle - Step;
			if(Angle < -180) Angle += 360;
			FullCubeZRotation.Angle = Angle;
			}

		// positive Y change
		if(DeltaY > 0)
			{
			double Angle = FullCubeXRotation.Angle + Step;
			if(Angle > 180) Angle -= 360;
			FullCubeXRotation.Angle = Angle;
			}
		// negative y change
		else if(DeltaY < 0)
			{
			double Angle = FullCubeXRotation.Angle - Step;
			if(Angle < -180) Angle += 360;
			FullCubeXRotation.Angle = Angle;
			}
		return;
		}

	/// <summary>
	/// Hit test
	/// </summary>
	/// <param name="p">Mouse position</param>
	private void HitTest(Point p)
		{
		// hit test
		VisualTreeHelper.HitTest(CubeViewPort3D, null, new HitTestResultCallback(ResultCallBack), new PointHitTestParameters(p));
		return;
		}

	/// <summary>
	/// Hit test call back method
	/// </summary>
	/// <param name="Result"></param>
	/// <returns></returns>
	private HitTestResultBehavior ResultCallBack(HitTestResult Result)
		{
		// test for hit on movable face
		if(Result.VisualHit.GetType() == typeof(ModelVisual3DCube))
			{
			ModelVisual3DCube Model = (ModelVisual3DCube) Result.VisualHit;
			BlockFace3D Hit = Model.BlockFace;
			if(Hit.FaceNo >= 0)
				{
				// save the hit and stop further action
				HitList.Add(Hit);
				return HitTestResultBehavior.Stop;
				}
			}

		// keep looking for movable face
		return HitTestResultBehavior.Continue;
		}

	/// <summary>
	/// scramble the cube
	/// </summary>
	/// <param name="sender">sender</param>
	/// <param name="e">event arguments</param>
	private void ScrambleButtonClick(object sender, RoutedEventArgs e)
		{
		// ignore if rotation lock is on or user is painting the cube
		if(RotationLock || UserCubeActive) return;

		// reset info labels
		ResetInfoLabels(false);

		// perform 10 random moves
		int[] Steps = Cube.Scramble(10,  PastMoves.Count > 0 ? PastMoves[^1] : -1);
		NextMoves.AddRange(Steps);

		// display steps in info label
		string StepsText = Cube.ColorCodesToText(Steps);
		SolveLabel8.Content = StepsText;
		
		// trace
		#if DEBUG
		Trace.Write("Scramble: " + StepsText);
		#endif

		// perform first step
		RotateSide(NextMoves[0]);
		NextMoves.RemoveAt(0);
		return;
		}

	/// <summary>
	/// Solve one step
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void SolveStepButtonClick(object sender, RoutedEventArgs e)
		{
		if(!RotationLock && !UserCubeActive) SolveCube();
		return;
		}

	/// <summary>
	/// Auto solve the cube
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void AutoSolveButtonClick(object sender, RoutedEventArgs e)
		{
		if(!RotationLock && !UserCubeActive)
			{
			AutoSolve = true;
			SolveCube();
			}
		return;
		}

	/// <summary>
	/// Solve the cube one step at a time
	/// </summary>
	private void SolveCube()
		{
		try
			{
			// solution step
			SolutionStep SolveStep = RubiksCube3D.FullCube.NextSolutionStep();

			// done
			if(SolveStep.StepCode == StepCode.CubeIsSolved)
				{
				ResetInfoLabels(false);
				SolveLabel1.Content = Cube.StepCodeName[(int) SolveStep.StepCode];
				}

			// perform solution steps
			else
				{
				ResetInfoLabels(true);
				SolveLabel1.Content = Cube.StepCodeName[(int) SolveStep.StepCode];
				SolveLabel2.Content = Cube.GetBlockName(SolveStep.FaceNo);
				SolveLabel3.Content = SolveStep.Message;
				SolveLabel5.Content = Cube.FaceColorName[SolveStep.UpFaceColor];
				SolveLabel5.Background = Cube3D.FaceColor[SolveStep.UpFaceColor];
				SolveLabel7.Content = Cube.FaceColorName[SolveStep.FrontFaceColor];
				SolveLabel7.Background = Cube3D.FaceColor[SolveStep.FrontFaceColor];
				SolveLabel8.Content = Cube.RelativeCodesToText(SolveStep.UpFaceColor, SolveStep.FrontFaceColor, SolveStep.Steps);
				SolveLabel10.Content = Cube.ColorCodesToText(SolveStep.Steps);

				// add steps to next moves list
				NextMoves.AddRange(SolveStep.Steps);

				// set cube orientation
				SetUpAndFrontFace(true, SolveStep.FrontFaceColor, SolveStep.UpFaceColor);

				// initiate first step
				int NextMove = NextMoves[0];
				NextMoves.RemoveAt(0);
				RotateSide(NextMove);
				}
			}

		// exception
		catch (Exception Ex)
			{
			SolveLabel1.Content = "Solution step exception";
			SolveLabel2.Content = Ex.Message;
			}

		#if DEBUG
		Trace.Write(SolveLabel1.Content + ". " + SolveLabel2.Content + ". " + SolveLabel3.Content + ". " + SolveLabel5.Content +
			 ". " + SolveLabel7.Content + ". " + SolveLabel8.Content + ". " + SolveLabel10.Content);
		#endif
		return;
		}

	/// <summary>
	/// Reset info labels
	/// </summary>
	/// <param name="SolveState">State</param>
	private void ResetInfoLabels
			(
			bool SolveState
			)
		{
		SolveLabel1.Content = string.Empty;
		SolveLabel2.Content = string.Empty;
		SolveLabel3.Content = string.Empty;
		SolveLabel4.Content = SolveState ? "Up" : string.Empty;
		SolveLabel5.Content = string.Empty;
		SolveLabel5.Background = Brushes.LightYellow;
		SolveLabel6.Content = SolveState ? "Front" : string.Empty;
		SolveLabel7.Content = string.Empty;
		SolveLabel7.Background = Brushes.LightYellow;
		SolveLabel8.Content = string.Empty;
		SolveLabel9.Content = SolveState ? "Color" : string.Empty;
		SolveLabel10.Content = string.Empty;
		return;
		}

	/// <summary>
	/// Save solution
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void SaveSolutionButtonClick(object sender, RoutedEventArgs e)
		{
		// ignore if rotation lock is on or user is painting the cube
		if(RotationLock || UserCubeActive) return;

		// clone cube
		Cube TempCube = new(RubiksCube3D.FullCube);

		// solution steps file
		StreamWriter SolutionStepsFile = null;

		// solve the cube and save the results
		try
			{
			// open existing or create new trace file
			string FileName = Path.GetFullPath("SolutionSteps.txt");
			SolutionStepsFile = new StreamWriter(FileName);

			// write date and time
			SolutionStepsFile.WriteLine(string.Format("{0:yyyy}/{0:MM}/{0:dd} {0:HH}:{0:mm}:{0:ss} ", DateTime.Now));

			// write header
			SolutionStepsFile.WriteLine("Rubik's Cube definition");
			SolutionStepsFile.WriteLine("Front  Up");

			int Ptr = 0;
			for(int Face = 0; Face < Cube.FaceColors; Face++)
				{
				StringBuilder Str = new();
				Str.AppendFormat("{0,-15}[{1,-6}", Cube.SaveSolutionHeader[Face], Cube.FaceColorName[TempCube.FaceColor(Ptr++)]);
				for(int Index = 0; Index < 7; Index++) Str.AppendFormat(" {0,-6}", Cube.FaceColorName[TempCube.FaceColor(Ptr++)]);
				Str.Append(']');
				SolutionStepsFile.WriteLine(Str.ToString());
				}
	
			// write header
			SolutionStepsFile.WriteLine("Rubik's Cube solution steps");

			// count steps
			int TotalSteps = 0;

			// loop for solution steps
			for(;;)
				{
				// get next step
				SolutionStep SolveStep = TempCube.NextSolutionStep();

				// cube is solved
				if(SolveStep.StepCode == StepCode.CubeIsSolved)
					{
					SolutionStepsFile.WriteLine(Cube.StepCodeName[(int) SolveStep.StepCode]);
					SolutionStepsFile.WriteLine("Total number of steps: " + TotalSteps.ToString());
					break;
					}

				SolutionStepsFile.WriteLine(Cube.StepCodeName[(int) SolveStep.StepCode] + ". " +
					Cube.GetBlockName(SolveStep.FaceNo) + ". " + SolveStep.Message + ". " +
					Cube.FaceColorName[SolveStep.UpFaceColor] + ", " + Cube.FaceColorName[SolveStep.FrontFaceColor] + ", " +
					Cube.RelativeCodesToText(SolveStep.UpFaceColor, SolveStep.FrontFaceColor, SolveStep.Steps) + ", Color: " +
					Cube.ColorCodesToText(SolveStep.Steps));

				// perform the steps
				TempCube.RotateArray(SolveStep.Steps);
				TotalSteps += SolveStep.Steps.Length;
				}

			// close the file
			SolutionStepsFile.Close();
			SolutionStepsFile = null;

			// start default text editor and display the file
			Process Proc = new()
				{
				StartInfo = new ProcessStartInfo(FileName) { UseShellExecute = true }
				};
			Proc.Start();
			}

		catch (Exception Ex)
			{
			if(SolutionStepsFile != null) SolutionStepsFile.Close();
			MessageBox.Show(Ex.Message);
			}
		return;
		}

	/// <summary>
	/// Load solution file
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void LoadSolutionButtonClick(object sender, RoutedEventArgs e)
		{
		// ignore if rotation lock is on or user is painting the cube
		if(RotationLock || UserCubeActive) return;

		// open file dialog box
		System.Windows.Forms.OpenFileDialog OpenFile = new()
			{
			CheckFileExists = true,
			CheckPathExists = true,
			DefaultExt = ".txt",
			InitialDirectory = Environment.CurrentDirectory,
			Filter = "Solution files (*.txt)|*.txt",
			RestoreDirectory = true
			};

		// display dialog
		if (OpenFile.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

		// solution steps file
		StreamReader SolutionStepsFile = null;

		// solve the cube and save the results
		try
			{
			// face and color array
			int[] ColorFaceArray = new int[Cube.MovableFaces];
			int FaceNo = 0;

			// open existing file
			SolutionStepsFile = new StreamReader(OpenFile.FileName);

			// look for cube's definition
			while(FaceNo < Cube.MovableFaces)
				{
				// read next line
				string Line = SolutionStepsFile.ReadLine();
				if(Line == null) break;
				if(Line.Length == 0) continue;

				// look for [
				int Ptr1 = Line.IndexOf('[');
				if(Ptr1 < 0) continue;
				Ptr1++;

				// look for ]
				int Ptr2 = Line.IndexOf(']', Ptr1);
				if(Ptr2 < 0) continue;

				// break the area between [ and ]
				string[] FldArray = Line[Ptr1..Ptr2].Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
				if(FldArray.Length != 8) throw new ApplicationException("Load solution. Each line must have 8 fields");

				// loop for fields
				for(int FldIndex = 0; FldIndex < 8; FldIndex++)
					{
					// color code
					int ColorCode = "WwBbRrGgOoYy".IndexOf(FldArray[FldIndex][0]);
					if(ColorCode < 0) throw new ApplicationException("Load solution. Color must start with W, B, R, G, O, Y");
					ColorFaceArray[FaceNo++] = ColorCode / 2;
					}
				}

			// close the file
			SolutionStepsFile.Close();
			SolutionStepsFile = null;

			// test color array
			RubiksCube3D.FullCube.ColorArray = ColorFaceArray;

			// restore face array		
			RubiksCube3D.SetColorOfAllFaces();
			}

		catch (Exception Ex)
			{
			if(SolutionStepsFile != null) SolutionStepsFile.Close();
			MessageBox.Show(Ex.Message);
			}

		return;
		}

	/// <summary>
	/// User want to set the cube equals to his own real cube
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event Arguments</param>
	private void UserCubeButtonClick(object sender, RoutedEventArgs e)
		{
		// ignore if rotation is active
		if(RotationLock) return;

		// start painting the cube to match user real cube
		if(!UserCubeActive)
			{
			ResetInfoLabels(false);
			SolveLabel1.Content = "User cube";
			SolveLabel2.Content = "Set Rubik's Cube Faces Color";
			SolveLabel3.Content = "Press \"End Coloring\" when done";
			UserCubeButton.Content = "End Coloring";
			UserCubeActive = true;
			UserColorArray = RubiksCube3D.FullCube.ColorArray;
			}

		// end of painting
		// test colors to make sure it is a valid cube
		else
			{
			try
				{
				// the Cube.ColorArray tests the validity of the colors
				// if there is an 
				RubiksCube3D.FullCube.ColorArray = UserColorArray;
				}
			catch(ApplicationException AppEx)
				{
				MessageBox.Show(AppEx.Message);
				return;
				}

			ResetInfoLabels(false);
			UserCubeButton.Content = "User Cube";
			UserCubeActive = false;
			}
		return;
		}

	/// <summary>
	/// Select color for user cube painting
	/// </summary>
	/// <param name="sender">Sender</param>
	/// <param name="e">Event arguments</param>
	private void SetColorButtonClick(object sender, RoutedEventArgs e)
		{
		// not initialization
		if(sender != null)
			{
			// seleced color
			int Selected = TagTranslator(sender);

			// no change
			if(Selected == UserCubeSelection) return;

			// save selected color
			UserCubeSelection = Selected;
			}

		// set thick border around selected color
		for(int Index = 0; Index < Cube.FaceColors; Index++)
			{
			UserColorButtons[Index].BorderThickness = Index == UserCubeSelection ? Cube3D.ThickBorder : Cube3D.ThinBorder;
			}

		// set background color of header
		SetColorHeading.Background = Cube3D.FaceColor[UserCubeSelection];
		return;
		}

	private static int TagTranslator
			(
			object sender
			)
		{
		return int.Parse((string) ((Button) sender).Tag);
		}

	/// <summary>
	/// Resize window
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnMainGridSizeChanged(object sender, SizeChangedEventArgs e)
		{
		CubeGrid.Width = RubiksCubeWindow.ActualWidth - ButtonsPanelWidth - 20;
		CubeGrid.Height = RubiksCubeWindow.ActualHeight - InfoPanelHeight - 40;
		return;
		}
	}
}
