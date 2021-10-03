using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

[Library( "ent_car", Title = "Car", Spawnable = true )]
public partial class CarEntity : Prop, IUse
{
	[ConVar.Replicated( "debug_car" )]
	public static bool debug_car { get; set; } = false;

	[ConVar.Replicated( "car_accelspeed" )]
	public static float car_accelspeed { get; set; } = 500.0f;

	private CarWheel frontLeft;
	private CarWheel frontRight;
	private CarWheel backLeft;
	private CarWheel backRight;

	private float frontLeftDistance;
	private float frontRightDistance;
	private float backLeftDistance;
	private float backRightDistance;

	private bool frontWheelsOnGround;
	private bool backWheelsOnGround;
	private float accelerateDirection;
	private float airRoll;
	private float airTilt;
	private float grip;
	private TimeSince timeSinceDriverLeft;

	[Net] private float WheelSpeed { get; set; }
	[Net] private float TurnDirection { get; set; }
	[Net] private float AccelerationTilt { get; set; }
	[Net] private float TurnLean { get; set; }

	[Net] public float MovementSpeed { get; private set; }
	[Net] public bool Grounded { get; private set; }

	private struct InputState
	{
		public float throttle;
		public float turning;
		public float breaking;
		public float tilt;
		public float roll;

		public void Reset()
		{
			throttle = 0;
			turning = 0;
			breaking = 0;
			tilt = 0;
			roll = 0;
		}
	}

	private InputState currentInput;


	private static Random random = new Random();
	public string plateP1 { get; private set; }
	public string plateP2 { get; private set; }
	public string plateP3 { get; private set; }
	public string plate { get; private set; }
	private static string plateModel = "models/car/plate/sym/{{name}}.vmdl";

	private static string getLetterModel(char c)
	{
		return plateModel.Replace( "{{name}}", c.ToString() );
	}

	

	private List<ModelEntity> LettersFront = new List<ModelEntity>();
	private List<ModelEntity> LettersRear = new List<ModelEntity>();

	private static string RandomString( int length, bool letters = true, bool numbers = true )
	{

		string chars = (letters ? "azertyuiopqsdfghjklxcvbn" : "") + (numbers ? "123456789" : "");
		return new string( Enumerable.Repeat( chars, length )
		  .Select( s => s[random.Next( s.Length )] ).ToArray() );
	}

	public CarEntity()
	{
		plateP1 = RandomString( 2, true ,false) ;
		plateP2 = RandomString( 3, false, true );
		plateP3 = RandomString( 2, true, false );
		plate = $"{plateP1}-{plateP2}-{plateP3}";
		plate = "NN-987-NN";

		frontLeft = new CarWheel( this );
		frontRight = new CarWheel( this );
		backLeft = new CarWheel( this );
		backRight = new CarWheel( this );
	}

	[Net] public Player driver { get; private set; }

	private ModelEntity chassis_axle_rear;
	private ModelEntity chassis_axle_front;
	private ModelEntity wheel0;
	private ModelEntity wheel1;
	private ModelEntity wheel2;
	private ModelEntity wheel3;
	private ModelEntity plateFront;
	private ModelEntity plateRear;


	// Override Rayzox57

	// POTENTIAL ARTIST (CHANGE IN SPAWN)
	public string artistName { get; protected set; } = "FacePunch";
		public string artistUrl { get; protected set; } = "https://facepunch.com";
		public string artistModelUrl { get; protected set; } = "";

		// MODELS (CHANGE IN SPAWN)
		protected string modelName { get; set; } = "models/car/car.vmdl";

	// MODELS P2 (CHANGE IN CLIENTSPAWN)

		protected int skin { get; set; } = -1;

		protected string modelNameFuelTank { get; set; } = "entities/modular_vehicle/vehicle_fuel_tank.vmdl";
		protected string modelNameChassisFront { get; set; } = "entities/modular_vehicle/chassis_axle_front.vmdl";
		protected string modelNameWheelFrontRight { get; set; } = "entities/modular_vehicle/wheel_a.vmdl";
		protected string modelNameWheelFrontLeft { get; set; } = "entities/modular_vehicle/wheel_a.vmdl";
		protected string modelNameChassisSteering { get; set; } = "entities/modular_vehicle/chassis_steering.vmdl";

		protected string modelNameChassisRear { get; set; } = "entities/modular_vehicle/chassis_axle_rear.vmdl";
		protected string modelNameChassisTransmission { get; set; } = "entities/modular_vehicle/chassis_transmission.vmdl";
		protected string modelNameWheelRearRight { get; set; } = "entities/modular_vehicle/wheel_a.vmdl";
		protected string modelNameWheelRearLeft { get; set; } = "entities/modular_vehicle/wheel_a.vmdl";

		// VISIBLE (CHANGE IN CLIENTSPAWN)
		protected bool FuelTankShow { get; set; } = true;
		protected bool ChassisFrontShow { get; set; } = true;
		protected bool WheelGlobalShow { get; set; } = true;
		protected bool WheelFrontRightShow { get; set; } = true;
		protected bool WheelFrontLeftShow { get; set; } = true;
		protected bool ChassisSteeringShow { get; set; } = true;
		protected bool ChassisRearShow { get; set; } = true;
		protected bool ChassisTransmissionShow { get; set; } = true;
		protected bool WheelRearLeftShow { get; set; } = true;
		protected bool WheelRearRightShow { get; set; } = true;
		protected bool PlateGlobalShow { get; set; } = true;
		protected bool PlateFrontShow { get; set; } = true;
		protected bool PlateRearShow { get; set; } = true;


	// BASE POSITION
	protected Vector3 BaseModelPosition { get; set; } = new Vector3( 0, 0, 0 );

	// POSITION // ROTATION (CHANGE IN CLIENTSPAWN)
	protected Vector3 FuelTankPosition { get; set; } = new Vector3( 0.75f, 0, 0 );
		protected Vector3 ChassisFrontPosition { get; set; } = new Vector3( 1.05f, 0, 0.35f );

		protected Vector3 WheelFrontRightPosition { get; set; } = Vector3.Zero;
		protected Rotation WheelFrontRightRotation { get; set; } = Rotation.From( 0, 180, 0 );

		protected Vector3 WheelFrontLeftPosition { get; set; } = Vector3.Zero;
		protected Rotation WheelFrontLeftRotation { get; set; } = Rotation.From( 0, 0, 0 );

		protected Vector3 ChassisSteeringPosition { get; set; } = Vector3.Zero;
		protected Rotation ChassisSteeringRotation { get; set; } = Rotation.From( -90, 180, 0 );

		protected Vector3 ChassisRearPosition { get; set; } = new Vector3( -1.05f, 0, 0.35f );

		protected Vector3 ChassisTransmissionPosition { get; set; } = Vector3.Zero;
		protected Rotation ChassisTransmissionRotation { get; set; } = Rotation.From( -90, 180, 0 );

		protected float WheelRearLeftPosition { get; set; } = (0.7f * 40);
		protected Rotation WheelRearLeftRotation { get; set; } = Rotation.From( 0, 90, 0 );

		protected float WheelRearRightPosition { get; set; } = (0.7f * 40);
		protected Rotation WheelRearRightRotation { get; set; } = Rotation.From( 0, -90, 0 );


		protected Vector3 PlateFrontPosition { get; set; } = new Vector3( 0.6f, -0.0f, 0.3f );
		protected Rotation PlateFrontRotation { get; set; } = Rotation.From( 90, 90, 90 );

		protected Vector3 PlateRearPosition { get; set; } = new Vector3( -2.0f, -0.0f, 0.3f );
		protected Rotation PlateRearRotation { get; set; } = Rotation.From( 90, -90, 90 );


	// End Override

	public override void Spawn()
	{
		base.Spawn();
		SetModel( modelName );

		if ( skin > -1 ) SetMaterialGroup( skin );
		
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		SetInteractsExclude( CollisionLayer.Player );
		EnableSelfCollisions = false;

		var trigger = new ModelEntity
		{
			Parent = this,
			Position = Position,
			Rotation = Rotation,
			EnableTouch = true,
			CollisionGroup = CollisionGroup.Trigger,
			Transmit = TransmitType.Never,
			EnableSelfCollisions = false,
		};

		trigger.SetModel( modelName );
		trigger.SetupPhysicsFromModel( PhysicsMotionType.Keyframed, false );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();
		{
			var vehicle_fuel_tank = new ModelEntity();
			vehicle_fuel_tank.SetModel( modelNameFuelTank );
			vehicle_fuel_tank.Transform = Transform;
			vehicle_fuel_tank.Parent = this;
			vehicle_fuel_tank.LocalPosition = FuelTankPosition * 40.0f;
			vehicle_fuel_tank.RenderColor = new Color( 255,255,255, FuelTankShow ? 255 : 0 );

			{
				plateFront = new ModelEntity();
				plateFront.SetModel( "models/car/plate/car_plate.vmdl" );
				plateFront.SetParent(vehicle_fuel_tank,"", new Transform( PlateFrontPosition * 40.0f, PlateFrontRotation) );
				plateFront.RenderColor = new Color( 255, 255, 255, PlateGlobalShow && PlateFrontShow ? 255 : 0 );

				{

					float y = -0.18f;

					foreach(char c in plate)
					{
						if ( PlateGlobalShow && PlateFrontShow )
						{
							var letter = new ModelEntity();
							letter.SetModel( getLetterModel( c ) );
							letter.SetParent( plateFront, "", new Transform( new Vector3( 0, y, 0.001f ) * 40.0f, Rotation.From( 0, 0, 0 ) ) );
							LettersFront.Add( letter );
							y += 0.05f;
						}
					}
				}

			}

			{
				plateRear = new ModelEntity();
				plateRear.SetModel( "models/car/plate/car_plate.vmdl" );
				plateRear.SetParent( vehicle_fuel_tank, "", new Transform( PlateRearPosition * 40.0f, PlateRearRotation ) );
				plateRear.RenderColor = new Color( 255, 255, 255, PlateGlobalShow && PlateRearShow ? 255 : 0 );


				{

					float y = -0.18f;

					foreach ( char c in plate )
					{
						if ( PlateGlobalShow && PlateRearShow )
						{
							var letter = new ModelEntity();
						letter.SetModel( getLetterModel( c ) );
						letter.SetParent( plateRear, "", new Transform( new Vector3( 0, y, 0.001f ) * 40.0f, Rotation.From( 0, 0, 0 ) ) );
						LettersRear.Add( letter );
						y += 0.05f;
							}
					}
				}

			}

		}

		{
			chassis_axle_front = new ModelEntity();
			chassis_axle_front.SetModel( modelNameChassisFront );
			chassis_axle_front.Transform = Transform;
			chassis_axle_front.Parent = this;
			chassis_axle_front.LocalPosition = ChassisFrontPosition * 40.0f;
			chassis_axle_front.RenderColor = new Color( 255,255,255, ChassisFrontShow ? 255 : 0 );

			{
				wheel0 = new ModelEntity();
				wheel0.SetModel( modelNameWheelFrontRight );
				wheel0.SetParent( chassis_axle_front, "Wheel_Steer_R", new Transform( WheelFrontRightPosition, WheelFrontRightRotation ) );
				wheel0.RenderColor = new Color( 255,255,255, WheelGlobalShow && WheelFrontRightShow ? 255 : 0 );
			}

			{
				wheel1 = new ModelEntity();
				wheel1.SetModel( modelNameWheelFrontLeft );
				wheel1.SetParent( chassis_axle_front, "Wheel_Steer_L", new Transform( WheelFrontLeftPosition, WheelFrontLeftRotation ) );
				wheel1.RenderColor = new Color( 255,255,255, WheelGlobalShow && WheelFrontLeftShow ? 255 : 0 );
			}

			{
				var chassis_steering = new ModelEntity();
				chassis_steering.SetModel( modelNameChassisSteering );
				chassis_steering.SetParent( chassis_axle_front, "Axle_front_Center", new Transform( ChassisSteeringPosition, ChassisSteeringRotation ) );
				chassis_steering.RenderColor = new Color( 255,255,255, ChassisSteeringShow ? 255 : 0 );
			}
		}

		{
			chassis_axle_rear = new ModelEntity();
			chassis_axle_rear.SetModel( modelNameChassisRear );
			chassis_axle_rear.Transform = Transform;
			chassis_axle_rear.Parent = this;
			chassis_axle_rear.LocalPosition = ChassisRearPosition * 40.0f;
			chassis_axle_rear.RenderColor = new Color( 255,255,255, ChassisRearShow ? 255 : 0 );

			{
				var chassis_transmission = new ModelEntity();
				chassis_transmission.SetModel( modelNameChassisTransmission );
				chassis_transmission.SetParent( chassis_axle_rear, "Axle_Rear_Center", new Transform( ChassisTransmissionPosition, ChassisTransmissionRotation ) );
				chassis_transmission.RenderColor = new Color( 255,255,255, ChassisTransmissionShow ? 255 : 0 );
			}

			{
				wheel2 = new ModelEntity();
				wheel2.SetModel( modelNameWheelRearLeft );
				wheel2.SetParent( chassis_axle_rear, "Axle_Rear_Center", new Transform( Vector3.Left * WheelRearLeftPosition, WheelRearLeftRotation ) );
				wheel2.RenderColor = new Color( 255,255,255, WheelGlobalShow && WheelRearLeftShow ? 255 : 0 );
			}

			{
				wheel3 = new ModelEntity();
				wheel3.SetModel( modelNameWheelRearRight );
				wheel3.SetParent( chassis_axle_rear, "Axle_Rear_Center", new Transform( Vector3.Right * WheelRearRightPosition, WheelRearRightRotation ) );
				wheel3.RenderColor = new Color( 255,255,255, WheelGlobalShow && WheelRearRightShow ? 255 : 0 );
			}


		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		if ( driver is SandboxPlayer player )
		{
			RemoveDriver( player );
		}
	}

	public void ResetInput()
	{
		currentInput.Reset();
	}

	[Event.Tick.Server]
	protected void Tick()
	{
		if ( driver is SandboxPlayer player )
		{
			if ( player.LifeState != LifeState.Alive || player.Vehicle != this )
			{
				RemoveDriver( player );
			}
		}
	}

	public override void Simulate( Client owner )
	{
		if ( owner == null ) return;
		if ( !IsServer ) return;

		using ( Prediction.Off() )
		{
			currentInput.Reset();

			if ( Input.Pressed( InputButton.Use ) )
			{
				if ( owner.Pawn is SandboxPlayer player && !player.IsUseDisabled() )
				{
					RemoveDriver( player );

					return;
				}
			}

			currentInput.throttle = (Input.Down( InputButton.Forward ) ? 1 : 0) + (Input.Down( InputButton.Back ) ? -1 : 0);
			currentInput.turning = (Input.Down( InputButton.Left ) ? 1 : 0) + (Input.Down( InputButton.Right ) ? -1 : 0);
			currentInput.breaking = (Input.Down( InputButton.Jump ) ? 1 : 0);
			currentInput.tilt = (Input.Down( InputButton.Run ) ? 1 : 0) + (Input.Down( InputButton.Duck ) ? -1 : 0);
			currentInput.roll = (Input.Down( InputButton.Left ) ? 1 : 0) + (Input.Down( InputButton.Right ) ? -1 : 0);
		}
	}

	[Event.Physics.PreStep]
	public void OnPrePhysicsStep()
	{
		if ( !IsServer )
			return;

		var selfBody = PhysicsBody;
		if ( !selfBody.IsValid() )
			return;

		var body = selfBody.SelfOrParent;
		if ( !body.IsValid() )
			return;

		var dt = Time.Delta;

		body.DragEnabled = false;

		var rotation = selfBody.Rotation;

		accelerateDirection = currentInput.throttle.Clamp( -1, 1 ) * (1.0f - currentInput.breaking);
		TurnDirection = TurnDirection.LerpTo( currentInput.turning.Clamp( -1, 1 ), 1.0f - MathF.Pow( 0.001f, dt ) );

		airRoll = airRoll.LerpTo( currentInput.roll.Clamp( -1, 1 ), 1.0f - MathF.Pow( 0.0001f, dt ) );
		airTilt = airTilt.LerpTo( currentInput.tilt.Clamp( -1, 1 ), 1.0f - MathF.Pow( 0.0001f, dt ) );

		float targetTilt = 0;
		float targetLean = 0;

		var localVelocity = rotation.Inverse * body.Velocity;

		if ( backWheelsOnGround || frontWheelsOnGround )
		{
			var forwardSpeed = MathF.Abs( localVelocity.x );
			var speedFraction = MathF.Min( forwardSpeed / 500.0f, 1 );

			targetTilt = accelerateDirection.Clamp( -1.0f, 1.0f );
			targetLean = speedFraction * TurnDirection;
		}

		AccelerationTilt = AccelerationTilt.LerpTo( targetTilt, 1.0f - MathF.Pow( 0.01f, dt ) );
		TurnLean = TurnLean.LerpTo( targetLean, 1.0f - MathF.Pow( 0.01f, dt ) );

		if ( backWheelsOnGround )
		{
			var forwardSpeed = MathF.Abs( localVelocity.x );
			var speedFactor = 1.0f - (forwardSpeed / 5000.0f).Clamp( 0.0f, 1.0f );
			var acceleration = speedFactor * (accelerateDirection < 0.0f ? car_accelspeed * 0.5f : car_accelspeed) * accelerateDirection * dt;
			var impulse = rotation * new Vector3( acceleration, 0, 0 );
			body.Velocity += impulse;
		}

		RaycastWheels( rotation, true, out frontWheelsOnGround, out backWheelsOnGround, dt );
		var onGround = frontWheelsOnGround || backWheelsOnGround;
		var fullyGrounded = (frontWheelsOnGround && backWheelsOnGround);
		Grounded = onGround;

		if ( fullyGrounded )
		{
			body.Velocity += PhysicsWorld.Gravity * dt;
		}

		body.GravityScale = fullyGrounded ? 0 : 1;

		bool canAirControl = false;

		var v = rotation * localVelocity.WithZ( 0 );
		var vDelta = MathF.Pow( (v.Length / 1000.0f).Clamp( 0, 1 ), 5.0f ).Clamp( 0, 1 );
		if ( vDelta < 0.01f ) vDelta = 0;

		if ( debug_car )
		{
			DebugOverlay.Line( body.MassCenter, body.MassCenter + rotation.Forward.Normal * 100, Color.White, 0, false );
			DebugOverlay.Line( body.MassCenter, body.MassCenter + v.Normal * 100, Color.Green, 0, false );
		}

		var angle = (rotation.Forward.Normal * MathF.Sign( localVelocity.x )).Normal.Dot( v.Normal ).Clamp( 0.0f, 1.0f );
		angle = angle.LerpTo( 1.0f, 1.0f - vDelta );
		grip = grip.LerpTo( angle, 1.0f - MathF.Pow( 0.001f, dt ) );

		if ( debug_car )
		{
			DebugOverlay.ScreenText( new Vector2( 200, 200 ), $"{grip}" );
		}

		var angularDamping = 0.0f;
		angularDamping = angularDamping.LerpTo( 5.0f, grip );

		body.LinearDamping = 0.0f;
		body.AngularDamping = fullyGrounded ? angularDamping : 0.5f;

		if ( onGround )
		{
			localVelocity = rotation.Inverse * body.Velocity;
			WheelSpeed = localVelocity.x;
			var turnAmount = frontWheelsOnGround ? (MathF.Sign( localVelocity.x ) * 25.0f * CalculateTurnFactor( TurnDirection, MathF.Abs( localVelocity.x ) ) * dt) : 0.0f;
			body.AngularVelocity += rotation * new Vector3( 0, 0, turnAmount );

			airRoll = 0;
			airTilt = 0;

			var forwardGrip = 0.1f;
			forwardGrip = forwardGrip.LerpTo( 0.9f, currentInput.breaking );
			body.Velocity = VelocityDamping( Velocity, rotation, new Vector3( forwardGrip, grip, 0 ), dt );
		}
		else
		{
			var s = selfBody.Position + (rotation * selfBody.LocalMassCenter);
			var tr = Trace.Ray( s, s + rotation.Down * 50 )
				.Ignore( this )
				.Run();

			if ( debug_car )
				DebugOverlay.Line( tr.StartPos, tr.EndPos, tr.Hit ? Color.Red : Color.Green );

			canAirControl = !tr.Hit;
		}

		if ( canAirControl && (airRoll != 0 || airTilt != 0) )
		{
			var offset = 50 * Scale;
			var s = selfBody.Position + (rotation * selfBody.LocalMassCenter) + (rotation.Right * airRoll * offset) + (rotation.Down * (10 * Scale));
			var tr = Trace.Ray( s, s + rotation.Up * (25 * Scale) )
				.Ignore( this )
				.Run();

			if ( debug_car )
				DebugOverlay.Line( tr.StartPos, tr.EndPos );

			bool dampen = false;

			if ( currentInput.roll.Clamp( -1, 1 ) != 0 )
			{
				var force = tr.Hit ? 400.0f : 100.0f;
				var roll = tr.Hit ? currentInput.roll.Clamp( -1, 1 ) : airRoll;
				body.ApplyForceAt( selfBody.MassCenter + rotation.Left * (offset * roll), (rotation.Down * roll) * (roll * (body.Mass * force)) );

				if ( debug_car )
					DebugOverlay.Sphere( selfBody.MassCenter + rotation.Left * (offset * roll), 8, Color.Red );

				dampen = true;
			}

			if ( !tr.Hit && currentInput.tilt.Clamp( -1, 1 ) != 0 )
			{
				var force = 200.0f;
				body.ApplyForceAt( selfBody.MassCenter + rotation.Forward * (offset * airTilt), (rotation.Down * airTilt) * (airTilt * (body.Mass * force)) );

				if ( debug_car )
					DebugOverlay.Sphere( selfBody.MassCenter + rotation.Forward * (offset * airTilt), 8, Color.Green );

				dampen = true;
			}

			if ( dampen )
				body.AngularVelocity = VelocityDamping( body.AngularVelocity, rotation, 0.95f, dt );
		}

		localVelocity = rotation.Inverse * body.Velocity;
		MovementSpeed = localVelocity.x;
	}

	private static float CalculateTurnFactor( float direction, float speed )
	{
		var turnFactor = MathF.Min( speed / 500.0f, 1 );
		var yawSpeedFactor = 1.0f - (speed / 1000.0f).Clamp( 0, 0.6f );

		return direction * turnFactor * yawSpeedFactor;
	}

	private static Vector3 VelocityDamping( Vector3 velocity, Rotation rotation, Vector3 damping, float dt )
	{
		var localVelocity = rotation.Inverse * velocity;
		var dampingPow = new Vector3( MathF.Pow( 1.0f - damping.x, dt ), MathF.Pow( 1.0f - damping.y, dt ), MathF.Pow( 1.0f - damping.z, dt ) );
		return rotation * (localVelocity * dampingPow);
	}

	private void RaycastWheels( Rotation rotation, bool doPhysics, out bool frontWheels, out bool backWheels, float dt )
	{
		float forward = 42;
		float right = 32;

		var frontLeftPos = rotation.Forward * forward + rotation.Right * right + rotation.Up * 20;
		var frontRightPos = rotation.Forward * forward - rotation.Right * right + rotation.Up * 20;
		var backLeftPos = -rotation.Forward * forward + rotation.Right * right + rotation.Up * 20;
		var backRightPos = -rotation.Forward * forward - rotation.Right * right + rotation.Up * 20;

		var tiltAmount = AccelerationTilt * 2.5f;
		var leanAmount = TurnLean * 2.5f;

		float length = 20.0f;

		frontWheels =
			frontLeft.Raycast( length + tiltAmount - leanAmount, doPhysics, frontLeftPos * Scale, ref frontLeftDistance, dt ) |
			frontRight.Raycast( length + tiltAmount + leanAmount, doPhysics, frontRightPos * Scale, ref frontRightDistance, dt );

		backWheels =
			backLeft.Raycast( length - tiltAmount - leanAmount, doPhysics, backLeftPos * Scale, ref backLeftDistance, dt ) |
			backRight.Raycast( length - tiltAmount + leanAmount, doPhysics, backRightPos * Scale, ref backRightDistance, dt );
	}

	float wheelAngle = 0.0f;
	float wheelRevolute = 0.0f;

	[Event.Frame]
	public void OnFrame()
	{
		wheelAngle = wheelAngle.LerpTo( TurnDirection * 25, 1.0f - MathF.Pow( 0.001f, Time.Delta ) );
		wheelRevolute += (WheelSpeed / (14.0f * Scale)).RadianToDegree() * Time.Delta;

		var wheelRotRight = Rotation.From( -wheelAngle, 180, -wheelRevolute );
		var wheelRotLeft = Rotation.From( wheelAngle, 0, wheelRevolute );
		var wheelRotBackRight = Rotation.From( 0, 90, -wheelRevolute );
		var wheelRotBackLeft = Rotation.From( 0, -90, wheelRevolute );

		RaycastWheels( Rotation, false, out _, out _, Time.Delta );

		float frontOffset = 20.0f - Math.Min( frontLeftDistance, frontRightDistance );
		float backOffset = 20.0f - Math.Min( backLeftDistance, backRightDistance );

		chassis_axle_front.SetBoneTransform( "Axle_front_Center", new Transform( Vector3.Up * frontOffset ), false );
		chassis_axle_rear.SetBoneTransform( "Axle_Rear_Center", new Transform( Vector3.Up * backOffset ), false );

		wheel0.LocalRotation = wheelRotRight;
		wheel1.LocalRotation = wheelRotLeft;
		wheel2.LocalRotation = wheelRotBackRight;
		wheel3.LocalRotation = wheelRotBackLeft;
	}

	private void RemoveDriver( SandboxPlayer player )
	{
		driver = null;
		timeSinceDriverLeft = 0;

		ResetInput();

		if ( !player.IsValid() )
			return;

		player.Vehicle = null;
		player.VehicleController = null;
		player.VehicleAnimator = null;
		player.VehicleCamera = null;
		player.Parent = null;

		if ( player.PhysicsBody.IsValid() )
		{
			player.PhysicsBody.Enabled = true;
			player.PhysicsBody.Position = player.Position;
		}
	}

	public bool OnUse( Entity user )
	{
		if ( user is SandboxPlayer player && player.Vehicle == null && timeSinceDriverLeft > 1.0f )
		{
			player.Vehicle = this;
			player.VehicleController = new CarController();
			player.VehicleAnimator = new CarAnimator();
			player.VehicleCamera = new CarCamera();
			player.Parent = this;
			player.LocalPosition = Vector3.Up * 10;
			player.LocalRotation = Rotation.Identity;
			player.LocalScale = 1;
			player.PhysicsBody.Enabled = false;

			driver = player;
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return this.State == CarState.UNLOCK && driver == null;
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if ( !IsServer )
			return;

		var body = PhysicsBody;
		if ( !body.IsValid() )
			return;

		body = body.SelfOrParent;
		if ( !body.IsValid() )
			return;

		if ( other is SandboxPlayer player && player.Vehicle == null )
		{
			var speed = body.Velocity.Length;
			var forceOrigin = Position + Rotation.Down * Rand.Float( 20, 30 );
			var velocity = (player.Position - forceOrigin).Normal * speed;
			var angularVelocity = body.AngularVelocity;

			OnPhysicsCollision( new CollisionEventData
			{
				Entity = player,
				Pos = player.Position + Vector3.Up * 50,
				Velocity = velocity,
				PreVelocity = velocity,
				PostVelocity = velocity,
				PreAngularVelocity = angularVelocity,
				Speed = speed,
			} );
		}
	}

	protected override void OnPhysicsCollision( CollisionEventData eventData )
	{
		if ( !IsServer )
			return;

		if ( eventData.Entity is SandboxPlayer player && player.Vehicle != null )
		{
			return;
		}

		var propData = GetModelPropData();

		var minImpactSpeed = propData.MinImpactDamageSpeed;
		if ( minImpactSpeed <= 0.0f ) minImpactSpeed = 500;

		var impactDmg = propData.ImpactDamage;
		if ( impactDmg <= 0.0f ) impactDmg = 10;

		var speed = eventData.Speed;

		if ( speed > minImpactSpeed )
		{
			if ( eventData.Entity.IsValid() && eventData.Entity != this )
			{
				var damage = speed / minImpactSpeed * impactDmg * 1.2f;
				eventData.Entity.TakeDamage( DamageInfo.Generic( damage )
					.WithFlag( DamageFlags.PhysicsImpact )
					.WithFlag( DamageFlags.Vehicle )
					.WithAttacker( driver != null ? driver : this, driver != null ? this : null )
					.WithPosition( eventData.Pos )
					.WithForce( eventData.PreVelocity ) );

				if ( eventData.Entity.LifeState == LifeState.Dead && eventData.Entity is not SandboxPlayer )
				{
					PhysicsBody.Velocity = eventData.PreVelocity;
					PhysicsBody.AngularVelocity = eventData.PreAngularVelocity;
				}
			}
		}
	}
}
