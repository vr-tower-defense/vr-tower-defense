//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Action_Pose p_default_Pose;
        
        private static SteamVR_Action_Skeleton p_default_SkeletonLeftHand;
        
        private static SteamVR_Action_Skeleton p_default_SkeletonRightHand;
        
        private static SteamVR_Action_Boolean p_default_HeadsetOnHead;
        
        private static SteamVR_Action_Vector2 p_default_Dial;
        
        private static SteamVR_Action_Boolean p_default_DialClick;
        
        private static SteamVR_Action_Boolean p_default_TriggerClick;
        
        private static SteamVR_Action_Boolean p_default_MenuButtonClick;
        
        private static SteamVR_Action_Boolean p_default_InteractWithUI;
        
        private static SteamVR_Action_Boolean p_default_GrabGrip;
        
        private static SteamVR_Action_Boolean p_default_GrapPinch;
        
        private static SteamVR_Action_Boolean p_default_Teleport;
        
        private static SteamVR_Action_Boolean p_default_PlayTutorial;
        
        private static SteamVR_Action_Vibration p_default_Haptic;
        
        private static SteamVR_Action_Vector2 p_platformer_Move;
        
        private static SteamVR_Action_Boolean p_platformer_Jump;
        
        private static SteamVR_Action_Vector2 p_buggy_Steering;
        
        private static SteamVR_Action_Single p_buggy_Throttle;
        
        private static SteamVR_Action_Boolean p_buggy_Brake;
        
        private static SteamVR_Action_Boolean p_buggy_Reset;
        
        private static SteamVR_Action_Pose p_mixedreality_ExternalCamera;
        
        public static SteamVR_Action_Pose default_Pose
        {
            get
            {
                return SteamVR_Actions.p_default_Pose.GetCopy<SteamVR_Action_Pose>();
            }
        }
        
        public static SteamVR_Action_Skeleton default_SkeletonLeftHand
        {
            get
            {
                return SteamVR_Actions.p_default_SkeletonLeftHand.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Skeleton default_SkeletonRightHand
        {
            get
            {
                return SteamVR_Actions.p_default_SkeletonRightHand.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Boolean default_HeadsetOnHead
        {
            get
            {
                return SteamVR_Actions.p_default_HeadsetOnHead.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vector2 default_Dial
        {
            get
            {
                return SteamVR_Actions.p_default_Dial.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Boolean default_DialClick
        {
            get
            {
                return SteamVR_Actions.p_default_DialClick.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_TriggerClick
        {
            get
            {
                return SteamVR_Actions.p_default_TriggerClick.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_MenuButtonClick
        {
            get
            {
                return SteamVR_Actions.p_default_MenuButtonClick.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_InteractWithUI
        {
            get
            {
                return SteamVR_Actions.p_default_InteractWithUI.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_GrabGrip
        {
            get
            {
                return SteamVR_Actions.p_default_GrabGrip.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_GrapPinch
        {
            get
            {
                return SteamVR_Actions.p_default_GrapPinch.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_Teleport
        {
            get
            {
                return SteamVR_Actions.p_default_Teleport.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_PlayTutorial
        {
            get
            {
                return SteamVR_Actions.p_default_PlayTutorial.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vibration default_Haptic
        {
            get
            {
                return SteamVR_Actions.p_default_Haptic.GetCopy<SteamVR_Action_Vibration>();
            }
        }
        
        public static SteamVR_Action_Vector2 platformer_Move
        {
            get
            {
                return SteamVR_Actions.p_platformer_Move.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Boolean platformer_Jump
        {
            get
            {
                return SteamVR_Actions.p_platformer_Jump.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vector2 buggy_Steering
        {
            get
            {
                return SteamVR_Actions.p_buggy_Steering.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Single buggy_Throttle
        {
            get
            {
                return SteamVR_Actions.p_buggy_Throttle.GetCopy<SteamVR_Action_Single>();
            }
        }
        
        public static SteamVR_Action_Boolean buggy_Brake
        {
            get
            {
                return SteamVR_Actions.p_buggy_Brake.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean buggy_Reset
        {
            get
            {
                return SteamVR_Actions.p_buggy_Reset.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Pose mixedreality_ExternalCamera
        {
            get
            {
                return SteamVR_Actions.p_mixedreality_ExternalCamera.GetCopy<SteamVR_Action_Pose>();
            }
        }
        
        private static void InitializeActionArrays()
        {
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    SteamVR_Actions.default_Pose,
                    SteamVR_Actions.default_SkeletonLeftHand,
                    SteamVR_Actions.default_SkeletonRightHand,
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_Dial,
                    SteamVR_Actions.default_DialClick,
                    SteamVR_Actions.default_TriggerClick,
                    SteamVR_Actions.default_MenuButtonClick,
                    SteamVR_Actions.default_InteractWithUI,
                    SteamVR_Actions.default_GrabGrip,
                    SteamVR_Actions.default_GrapPinch,
                    SteamVR_Actions.default_Teleport,
                    SteamVR_Actions.default_PlayTutorial,
                    SteamVR_Actions.default_Haptic,
                    SteamVR_Actions.platformer_Move,
                    SteamVR_Actions.platformer_Jump,
                    SteamVR_Actions.buggy_Steering,
                    SteamVR_Actions.buggy_Throttle,
                    SteamVR_Actions.buggy_Brake,
                    SteamVR_Actions.buggy_Reset,
                    SteamVR_Actions.mixedreality_ExternalCamera};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_Pose,
                    SteamVR_Actions.default_SkeletonLeftHand,
                    SteamVR_Actions.default_SkeletonRightHand,
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_Dial,
                    SteamVR_Actions.default_DialClick,
                    SteamVR_Actions.default_TriggerClick,
                    SteamVR_Actions.default_MenuButtonClick,
                    SteamVR_Actions.default_InteractWithUI,
                    SteamVR_Actions.default_GrabGrip,
                    SteamVR_Actions.default_GrapPinch,
                    SteamVR_Actions.default_Teleport,
                    SteamVR_Actions.default_PlayTutorial,
                    SteamVR_Actions.platformer_Move,
                    SteamVR_Actions.platformer_Jump,
                    SteamVR_Actions.buggy_Steering,
                    SteamVR_Actions.buggy_Throttle,
                    SteamVR_Actions.buggy_Brake,
                    SteamVR_Actions.buggy_Reset,
                    SteamVR_Actions.mixedreality_ExternalCamera};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.ISteamVR_Action_Out[] {
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[] {
                    SteamVR_Actions.default_Haptic};
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[] {
                    SteamVR_Actions.default_Pose,
                    SteamVR_Actions.mixedreality_ExternalCamera};
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_DialClick,
                    SteamVR_Actions.default_TriggerClick,
                    SteamVR_Actions.default_MenuButtonClick,
                    SteamVR_Actions.default_InteractWithUI,
                    SteamVR_Actions.default_GrabGrip,
                    SteamVR_Actions.default_GrapPinch,
                    SteamVR_Actions.default_Teleport,
                    SteamVR_Actions.default_PlayTutorial,
                    SteamVR_Actions.platformer_Jump,
                    SteamVR_Actions.buggy_Brake,
                    SteamVR_Actions.buggy_Reset};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[] {
                    SteamVR_Actions.buggy_Throttle};
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[] {
                    SteamVR_Actions.default_Dial,
                    SteamVR_Actions.platformer_Move,
                    SteamVR_Actions.buggy_Steering};
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[] {
                    SteamVR_Actions.default_SkeletonLeftHand,
                    SteamVR_Actions.default_SkeletonRightHand};
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_HeadsetOnHead,
                    SteamVR_Actions.default_Dial,
                    SteamVR_Actions.default_DialClick,
                    SteamVR_Actions.default_TriggerClick,
                    SteamVR_Actions.default_MenuButtonClick,
                    SteamVR_Actions.default_InteractWithUI,
                    SteamVR_Actions.default_GrabGrip,
                    SteamVR_Actions.default_GrapPinch,
                    SteamVR_Actions.default_Teleport,
                    SteamVR_Actions.default_PlayTutorial,
                    SteamVR_Actions.platformer_Move,
                    SteamVR_Actions.platformer_Jump,
                    SteamVR_Actions.buggy_Steering,
                    SteamVR_Actions.buggy_Throttle,
                    SteamVR_Actions.buggy_Brake,
                    SteamVR_Actions.buggy_Reset};
        }
        
        private static void PreInitActions()
        {
            SteamVR_Actions.p_default_Pose = ((SteamVR_Action_Pose)(SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/default/in/Pose")));
            SteamVR_Actions.p_default_SkeletonLeftHand = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/default/in/SkeletonLeftHand")));
            SteamVR_Actions.p_default_SkeletonRightHand = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/default/in/SkeletonRightHand")));
            SteamVR_Actions.p_default_HeadsetOnHead = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/HeadsetOnHead")));
            SteamVR_Actions.p_default_Dial = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/default/in/Dial")));
            SteamVR_Actions.p_default_DialClick = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/DialClick")));
            SteamVR_Actions.p_default_TriggerClick = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/TriggerClick")));
            SteamVR_Actions.p_default_MenuButtonClick = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/MenuButtonClick")));
            SteamVR_Actions.p_default_InteractWithUI = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/InteractWithUI")));
            SteamVR_Actions.p_default_GrabGrip = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/GrabGrip")));
            SteamVR_Actions.p_default_GrapPinch = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/GrapPinch")));
            SteamVR_Actions.p_default_Teleport = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/Teleport")));
            SteamVR_Actions.p_default_PlayTutorial = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/PlayTutorial")));
            SteamVR_Actions.p_default_Haptic = ((SteamVR_Action_Vibration)(SteamVR_Action.Create<SteamVR_Action_Vibration>("/actions/default/out/Haptic")));
            SteamVR_Actions.p_platformer_Move = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/platformer/in/Move")));
            SteamVR_Actions.p_platformer_Jump = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/platformer/in/Jump")));
            SteamVR_Actions.p_buggy_Steering = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/buggy/in/Steering")));
            SteamVR_Actions.p_buggy_Throttle = ((SteamVR_Action_Single)(SteamVR_Action.Create<SteamVR_Action_Single>("/actions/buggy/in/Throttle")));
            SteamVR_Actions.p_buggy_Brake = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/buggy/in/Brake")));
            SteamVR_Actions.p_buggy_Reset = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/buggy/in/Reset")));
            SteamVR_Actions.p_mixedreality_ExternalCamera = ((SteamVR_Action_Pose)(SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/mixedreality/in/ExternalCamera")));
        }
    }
}
