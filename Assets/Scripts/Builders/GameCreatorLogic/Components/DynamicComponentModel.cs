using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;


public class ListItem
{
    public string itemID;
    public ItemData itemData;

    public ListItem(string itemID, ItemData itemData)
    {
        this.itemID = itemID;
        this.itemData = itemData;
    }
}

public class ItemModel
{
    public Item item;
    public ItemData itemData;

    public ItemModel(Item item, ItemData itemData)
    {
        this.item = item;
        this.itemData = itemData;
    }
}



[System.Serializable]
public class ItemData
{
    #region Data Variables
    public string ItemID;
    public string RuntimeItemID;    //unique runtime identifier for each item
    public string ParentID;     //identifier for parent of this item
    public string ItemType;
    public Vector3 Position;    //local position of the item
    public Vector3 Scale;
    public Quaternion Rotation;
    public bool isKinematic;
    public bool useGravity;
    public bool hasParent;
    public int parentIndex;
    public bool isLock;
    public bool isVisible;
    public bool shrinkHierarchy;
    public float mass;
    public string ItemName;
    public string RuntimeItemName;
    public string placedMaterialColor;
    public bool collectibleComponent;
    public bool rotatorComponent;
    public bool narratorComponent;
    public bool throwThingsComponent;
    public bool ninjaComponent;
    public bool spawnComponent;

    public bool healthComponent;
    public bool addForceComponent;

    public int collectiblePoints;
    public int healthPoints;

    //public bool isCurrentSpawn;//isCurrentSpawnKush
    public int addForcePoints;
    public RotatorComponentData rotatorComponentData;
    public CollectibleComponentData collectibleComponentData;
    public TimerComponentData timerComponentData;
    public TimeLimitComponentData timeLimitComponentData;
    public RandomNumberComponentData randomNumberComponentData;
    public HealthComponentData healthComponentData;
    public ToFroComponentData toFroComponentData;
    public TranslateComponentData translateComponentData;
    public ScalerComponentData scalerComponentData;
    public RotateComponentData rotateComponentData;
    public LadderComponentData ladderComponentData;
    public EnemyNPCComponentData enemyNPCComponentData;
    public ThrowThingsComponentData throwThingsComponentData;
    public AddForceComponentData addForceComponentData;
    public NarrationComponentData narrationComponentData;
    public TimerCountdownComponentData timerCountdownComponentData;
    public PowerProviderComponentData powerProviderComponentData;
    public AvatarChangerComponentData avatarChangerComponentData;
    public DoorKeyComponentData doorKeyComponentData;
    public ChestKeyComponentData chestKeyComponentData;
    public ElapsedTimeComponentData elapsedTimeComponentData;
    public HelpButtonComponentData helpButtonComponentData;
    public SituationChangerComponentData situationChangerComponentData;
    public SpecialItemComponentData speicalItemComponentData;
    public NinjaComponentData ninjaComponentData;
    public SpawnerItemComponentData spawnerComponentData;
    public DisplayMessageComponentData displayMessageComponentData;
    public QuizComponentData quizComponentData;
    #endregion

    public ItemData(string itemID, string itemType, string itemName, Vector3 position)
    {
        this.ItemID = itemID;
        this.RuntimeItemID = itemID + "_" + Guid.NewGuid().ToString();
        this.ParentID = "";
        this.ItemType = itemType;
        this.isKinematic = true;
        this.useGravity = true;
        this.isLock = false;
        //this.isCurrentSpawn= false;//isCurrentSpawnKush
        this.isVisible = true;
        this.shrinkHierarchy = false;
        this.mass = 1f;
        this.ItemName = itemName;
        this.RuntimeItemName = "";
        this.placedMaterialColor = ColorUtility.ToHtmlStringRGBA(Color.white);  //init default color as white
        this.Position = position;
        this.Scale = Vector3.one;
        this.Rotation = Quaternion.identity;

        this.collectibleComponent = false;
        this.rotatorComponent = false;
        this.healthComponent = false;
        this.addForceComponent = false;
        this.narratorComponent = false;
        this.throwThingsComponent = false;
        this.spawnComponent = false;
        this.collectiblePoints = 0;
        this.healthPoints = 0;


        this.rotatorComponentData = new RotatorComponentData();
        this.collectibleComponentData = new CollectibleComponentData();
        this.healthComponentData = new HealthComponentData();
        this.toFroComponentData = new ToFroComponentData();
        this.translateComponentData = new TranslateComponentData();
        this.scalerComponentData = new ScalerComponentData();
        this.rotateComponentData = new RotateComponentData();
        this.enemyNPCComponentData = new EnemyNPCComponentData();
        this.ladderComponentData = new LadderComponentData();
        this.addForceComponentData = new AddForceComponentData();
        this.timerComponentData = new TimerComponentData();
        this.timeLimitComponentData = new TimeLimitComponentData();
        this.randomNumberComponentData = new RandomNumberComponentData();
        this.narrationComponentData = new NarrationComponentData();
        this.timerCountdownComponentData = new TimerCountdownComponentData();
        this.powerProviderComponentData = new PowerProviderComponentData();
        this.avatarChangerComponentData = new AvatarChangerComponentData();
        this.doorKeyComponentData = new DoorKeyComponentData();
        this.chestKeyComponentData = new ChestKeyComponentData();
        this.elapsedTimeComponentData = new ElapsedTimeComponentData();
        this.helpButtonComponentData = new HelpButtonComponentData();
        this.throwThingsComponentData = new ThrowThingsComponentData();
        this.situationChangerComponentData = new SituationChangerComponentData();
        this.speicalItemComponentData = new SpecialItemComponentData();
        this.ninjaComponentData = new NinjaComponentData();
        this.spawnerComponentData = new SpawnerItemComponentData();
        this.displayMessageComponentData = new DisplayMessageComponentData();
        this.quizComponentData = new QuizComponentData();
    }

    public ItemData(ItemData itemData)
    {
        this.ItemID = itemData.ItemID;
        this.RuntimeItemID = itemData.ItemID + "_" + Guid.NewGuid().ToString();
        this.ParentID = itemData.ParentID;
        this.ItemType = itemData.ItemType;
        this.ItemName = itemData.ItemName;
        this.mass = itemData.mass;
        this.Position = itemData.Position;
        this.Scale = itemData.Scale;
        this.Rotation = itemData.Rotation;
        this.isKinematic = itemData.isKinematic;
        this.useGravity = itemData.useGravity;
        this.isLock = itemData.isLock;
        this.isVisible = itemData.isVisible;
        this.shrinkHierarchy = itemData.shrinkHierarchy;
        this.placedMaterialColor = itemData.placedMaterialColor;
        this.RuntimeItemName = itemData.RuntimeItemName;


        this.collectibleComponent = itemData.collectibleComponent;
        this.rotatorComponent = itemData.rotatorComponent;
        this.healthComponent = itemData.healthComponent;
        this.addForceComponent = itemData.addForceComponent;
        this.collectiblePoints = itemData.collectiblePoints;
        this.healthPoints = itemData.healthPoints;
        this.narratorComponent = itemData.narratorComponent;
        this.throwThingsComponent = itemData.throwThingsComponent;
        this.ninjaComponent = itemData.ninjaComponent;
        // this.spawnComponent = itemData.spawnComponent;



        this.rotatorComponentData = new RotatorComponentData(itemData.rotatorComponentData);
        this.toFroComponentData = new ToFroComponentData(itemData.toFroComponentData);
        this.translateComponentData = new TranslateComponentData(itemData.translateComponentData);
        this.scalerComponentData = new ScalerComponentData(itemData.scalerComponentData);
        this.rotateComponentData = new RotateComponentData(itemData.rotateComponentData);
        this.collectibleComponentData = new CollectibleComponentData(itemData.collectibleComponentData);
        this.healthComponentData = new HealthComponentData(itemData.healthComponentData);
        this.addForceComponentData = new AddForceComponentData(itemData.addForceComponentData);
        this.timerComponentData = new TimerComponentData(itemData.timerComponentData);
        this.timeLimitComponentData = new TimeLimitComponentData(itemData.timeLimitComponentData);
        this.randomNumberComponentData = new RandomNumberComponentData(itemData.randomNumberComponentData);
        this.narrationComponentData = new NarrationComponentData(itemData.narrationComponentData);
        this.timerCountdownComponentData = new TimerCountdownComponentData(itemData.timerCountdownComponentData);
        this.powerProviderComponentData = new PowerProviderComponentData(itemData.powerProviderComponentData);
        this.avatarChangerComponentData = new AvatarChangerComponentData(itemData.avatarChangerComponentData);
        this.enemyNPCComponentData = new EnemyNPCComponentData(itemData.enemyNPCComponentData);
        this.ladderComponentData = new LadderComponentData(itemData.ladderComponentData);
        this.elapsedTimeComponentData = new ElapsedTimeComponentData(itemData.elapsedTimeComponentData);
        this.doorKeyComponentData = new DoorKeyComponentData(itemData.doorKeyComponentData);
        this.chestKeyComponentData = new ChestKeyComponentData(itemData.chestKeyComponentData);
        this.helpButtonComponentData = new HelpButtonComponentData(itemData.helpButtonComponentData);
        this.throwThingsComponentData = new ThrowThingsComponentData(itemData.throwThingsComponentData);
        this.situationChangerComponentData = new SituationChangerComponentData(itemData.situationChangerComponentData);
        this.speicalItemComponentData = new SpecialItemComponentData(itemData.speicalItemComponentData);
        this.ninjaComponentData = new NinjaComponentData(itemData.ninjaComponentData);
        this.spawnerComponentData = new SpawnerItemComponentData(itemData.spawnerComponentData);
        this.displayMessageComponentData = new DisplayMessageComponentData(itemData.displayMessageComponentData);
        this.quizComponentData = new QuizComponentData(itemData.quizComponentData);
    }
}








namespace Models
{
    #region ItemComponents

    [System.Serializable]
    public class CollectibleComponentData
    {
        public bool IsActive;
        public float points;

        public CollectibleComponentData()
        {
            IsActive = false;
            points = 0f;
        }

        public void Reset()
        {
            IsActive = false;
            points = 0f;
        }

        public CollectibleComponentData(CollectibleComponentData data)
        {
            IsActive = data.IsActive;
            points = data.points;
        }
    }

    [System.Serializable]
    public class RotatorComponentData
    {
        public bool IsActive;
        public float speed;

        public RotatorComponentData()
        {
            IsActive = false;
            speed = 1;
        }
        public void Reset()
        {
            IsActive = false;
            speed = 1;
        }

        public RotatorComponentData(RotatorComponentData rotatorComponentData)
        {
            IsActive = rotatorComponentData.IsActive;
            speed = rotatorComponentData.speed;
        }
    }



    [System.Serializable]
    public class TranslateComponentData
    {
        public bool IsActive;
        public bool IsFacing;
        public float translateSpeed;
        public List<Vector3> translatePoint;
        public TranslateComponentData()
        {
            IsActive = false;
            IsFacing = false;
            translateSpeed = 10f;
            translatePoint = new List<Vector3>();
        }
        public void Reset()
        {
            IsActive = false;
            IsFacing = false;
            translateSpeed = 10f;
            translatePoint = new List<Vector3>();
        }
        public TranslateComponentData(TranslateComponentData translateComponentData)
        {
            IsActive = translateComponentData.IsActive;
            IsFacing = translateComponentData.IsFacing;
            translateSpeed = translateComponentData.translateSpeed;
            translatePoint = translateComponentData.translatePoint;
        }
    }
    [Serializable]
    public class ToFroComponentData
    {
        //public enum axis
        //{
        //    x_Axis, y_Axis, z_Axis
        //}
        //public bool IsActive;
        //public axis xyz_Axis;
        //public float distanceToCover;
        public bool IsActive;
        public bool shallLoop;
        public Vector3 defaultValue, maxValue;
        public float timeToAnimate;
        public AnimationCurve animationCurve;
        public int animationCurveIndex;
        public ToFroComponentData()
        {
            IsActive = false;
            //xyz_Axis = axis.x_Axis;
            //distanceToCover = 5f;
            shallLoop = false;
            defaultValue = new Vector3(1, 1, 1);
            maxValue = new Vector3(3, 3, 3);
            timeToAnimate = 1;
            animationCurveIndex = 0;
        }
        public void Reset()
        {
            IsActive = false;
            shallLoop = false;
            defaultValue = new Vector3(1, 1, 1);
            maxValue = new Vector3(3, 3, 3);
            timeToAnimate = 1;
            animationCurveIndex = 0;
        }
        public ToFroComponentData(ToFroComponentData toFroComponentData)
        {
            IsActive = toFroComponentData.IsActive;
            shallLoop = toFroComponentData.shallLoop;
            defaultValue = toFroComponentData.defaultValue;
            maxValue = toFroComponentData.maxValue;
            timeToAnimate = toFroComponentData.timeToAnimate;
            animationCurveIndex = toFroComponentData.animationCurveIndex;
        }
    }
    [System.Serializable]
    public class ScalerComponentData
    {
        public bool IsActive;
        public bool shallLoop;
        public Vector3 defaultScaleValue, maxScaleValue;
        public float timeToAnimate;
        public AnimationCurve animationCurve;
        public int animationCurveIndex;
        public ScalerComponentData()
        {
            IsActive = false;
            shallLoop = false;
            defaultScaleValue = new Vector3(1, 1, 1);
            maxScaleValue = new Vector3(3, 3, 3);
            timeToAnimate = 5;
            animationCurveIndex = 0;
        }
        public void Reset()
        {
            IsActive = false;
            shallLoop = false;
            defaultScaleValue = new Vector3(1, 1, 1);
            maxScaleValue = new Vector3(3, 3, 3);
            timeToAnimate = 5;
            animationCurveIndex = 0;
        }
        public ScalerComponentData(ScalerComponentData scalerComponentData)
        {
            IsActive = scalerComponentData.IsActive;
            shallLoop = scalerComponentData.shallLoop;
            defaultScaleValue = scalerComponentData.defaultScaleValue;
            maxScaleValue = scalerComponentData.maxScaleValue;
            timeToAnimate = scalerComponentData.timeToAnimate;
            animationCurveIndex = scalerComponentData.animationCurveIndex;
        }
    }
    [Serializable]
    public class RotateComponentData
    {
        public bool IsActive;
        public bool shallLoop;
        public Vector3 defaultValue, maxValue;
        public float timeToAnimate;
        public AnimationCurve animationCurve;
        public int animationCurveIndex;
        public RotateComponentData()
        {
            IsActive = false;
            shallLoop = false;
            defaultValue = new Vector3(1, 1, 1);
            maxValue = new Vector3(3, 3, 3);
            timeToAnimate = 1;
            animationCurveIndex = 0;
        }
        public void Reset()
        {
            IsActive = false;
            shallLoop = false;
            defaultValue = new Vector3(1, 1, 1);
            maxValue = new Vector3(3, 3, 3);
            timeToAnimate = 1;
            animationCurveIndex = 0;
        }
        public RotateComponentData(RotateComponentData rotateComponentData)
        {
            IsActive = rotateComponentData.IsActive;
            shallLoop = rotateComponentData.shallLoop;
            defaultValue = rotateComponentData.defaultValue;
            maxValue = rotateComponentData.maxValue;
            timeToAnimate = rotateComponentData.timeToAnimate;
            animationCurveIndex = rotateComponentData.animationCurveIndex;
        }
    }

    [System.Serializable]
    public class LadderComponentData
    {
        public bool IsActive;
        public float speed;
        public bool isLadder;
        public int animIndex;
        public LadderComponentData()
        {
            IsActive = false;
            animIndex = 0;
            speed = 3;
        }
        public void Reset()
        {
            IsActive = false;
            animIndex = 0;
            speed = 3;
        }

        public LadderComponentData(LadderComponentData data)
        {
            IsActive = data.IsActive;
            speed = data.speed;
            animIndex = data.animIndex;
        }
    }

    [System.Serializable]
    public class EnemyNPCComponentData
    {
        public bool IsActive;
        public float EnemyNPChealthValue;
        public float damageTaken;
        public string addTag = "Player";
        public EnemyNPCComponentData()
        {
            IsActive = false;
            EnemyNPChealthValue = 100;
            damageTaken = 50;
            addTag = "Player";
        }
        public void Reset()
        {
            IsActive = false;
            EnemyNPChealthValue = 100;
            damageTaken = 50;
            addTag = "Player";
        }

        public EnemyNPCComponentData(EnemyNPCComponentData data)
        {
            IsActive = data.IsActive;
            EnemyNPChealthValue = data.EnemyNPChealthValue;
            damageTaken = data.damageTaken;
            addTag = data.addTag;
        }

    }


    [System.Serializable]
    public class NarrationComponentData
    {
        public bool IsActive;
        public List<string> narrationsData;
        public List<TMPro.TMP_InputField> informationDataRewriting;
        public bool onStart;
        public float timeBwNarrations;
        public NarrationComponentData()
        {
            IsActive = false;
            onStart = false;
            narrationsData = new List<string>();
            informationDataRewriting = new List<TMPro.TMP_InputField>();
            timeBwNarrations = 1f;
        }
        public void Reset()
        {
            IsActive = false;
            onStart = false;
            narrationsData = new List<string>();
            informationDataRewriting = new List<TMPro.TMP_InputField>();
            timeBwNarrations = 1f;
        }

        public NarrationComponentData(NarrationComponentData data)
        {
            IsActive = data.IsActive;
            narrationsData = data.narrationsData;
            informationDataRewriting = data.informationDataRewriting;
            onStart = data.onStart;
            timeBwNarrations = data.timeBwNarrations;
        }
    }

    [System.Serializable]
    public class HelpButtonComponentData
    {
        public bool IsActive;
        public string titleHelpButton;
        public List<string> helpButtonData;
        public List<TMPro.TMP_InputField> informationDataRewriting;
        public HelpButtonComponentData()
        {
            IsActive = false;
            titleHelpButton = "Enter Title";
            helpButtonData = new List<string>();
            informationDataRewriting = new List<TMPro.TMP_InputField>();
        }
        public void Reset()
        {
            IsActive = false;
            titleHelpButton = "Enter Title";
            helpButtonData = new List<string>();
            informationDataRewriting = new List<TMPro.TMP_InputField>();
        }
        public HelpButtonComponentData(HelpButtonComponentData data)
        {
            IsActive = data.IsActive;
            titleHelpButton = data.titleHelpButton;
            helpButtonData = data.helpButtonData;
            informationDataRewriting = data.informationDataRewriting;
        }
    }


    [System.Serializable]
    public class TimerCountdownComponentData
    {
        public bool IsActive;
        public float setTimer;

        public TimerCountdownComponentData()
        {
            IsActive = false;
            setTimer = 3;
        }

        public void Reset()
        {
            IsActive = false;
            setTimer = 3;
        }

        public TimerCountdownComponentData(TimerCountdownComponentData data)
        {
            IsActive = data.IsActive;
            setTimer = data.setTimer;
        }
    }
    [System.Serializable]
    public class AddForceComponentData
    {
        public bool isActive;
        public int forceAmount;
        public Vector3 forceDirection;
        public int directionNumber;

        public AddForceComponentData()
        {
            isActive = false;
            forceAmount = 100;
            forceDirection = new Vector3(0f, 0f, -1f);
            directionNumber = 0;
        }

        public void Reset()
        {
            isActive = false;
            forceAmount = 100;
            forceDirection = new Vector3(0f, 0f, -1f);
            directionNumber = 0;
        }
        public AddForceComponentData(AddForceComponentData data)
        {
            isActive = data.isActive;
            forceAmount = data.forceAmount;
            forceDirection = data.forceDirection;
            directionNumber = data.directionNumber;
        }
    }

    [System.Serializable]
    public class TimerComponentData
    {
        public bool IsActive;
        public bool IsStart;
        public bool IsEnd;
        public float Timer;

        public TimerComponentData()
        {
            IsActive = false;
            IsStart = false;
            IsEnd = false;
            Timer = 0f;
        }
        public void Reset()
        {
            IsStart = false;
            IsEnd = false;
            IsActive = false;
            Timer = 0f;
        }
        public TimerComponentData(TimerComponentData data)
        {
            IsActive = data.IsActive;
            IsStart = data.IsStart;
            IsEnd = data.IsEnd;
            Timer = data.Timer;
        }
    }

    [System.Serializable]
    public class TimeLimitComponentData
    {
        public bool IsActive;
        public float TimerLimit;
        public string Purpose;
        public TimeLimitComponentData()
        {
            IsActive = false;
            TimerLimit = 0f;
            Purpose = "Timer Title";
        }
        public void Reset()
        {
            IsActive = false;
            TimerLimit = 0f;
            Purpose = "Timer Title";
        }
        public TimeLimitComponentData(TimeLimitComponentData data)
        {
            IsActive = data.IsActive;
            TimerLimit = data.TimerLimit;
            Purpose = data.Purpose;
        }
    }
    [System.Serializable]
    public class RandomNumberComponentData
    {
        public bool IsActive;
        public float minNumber;
        public float maxNumber;

        public RandomNumberComponentData()
        {
            IsActive = false;
            minNumber = 0;
            maxNumber = 0;
        }
        public void Reset()
        {
            IsActive = false;
            minNumber = 0;
            maxNumber = 0;
        }

        public RandomNumberComponentData(RandomNumberComponentData data)
        {
            IsActive = data.IsActive;
            minNumber = data.minNumber;
            maxNumber = data.maxNumber;
        }
    }

    [System.Serializable]
    public class DoorKeyComponentData
    {
        public bool IsActive;
        public static List<string> dropdownListing = new List<string>();
        public bool isKey;
        public bool isDoor;
        public List<string> doorValue = new List<string>();
        public string keyValue = "";
        public List<int> selectedOption = new List<int>();
        public int extraFields = 0;
        public List<TMPro.TMP_Dropdown> _extraValues = new List<TMPro.TMP_Dropdown>();

        public DoorKeyComponentData()
        {
            IsActive = false;
            isDoor = false;
            isKey = false;
            //  doorValue = "";
            doorValue.Clear();
            keyValue = "";
            selectedOption.Clear();
            extraFields = 0;
            //  dropdownListing.Clear();
        }
        public void Reset()
        {
            IsActive = false;
            isDoor = false;
            isKey = false;
            //  doorValue = "";
            doorValue.Clear();
            keyValue = "";
            selectedOption.Clear();
            extraFields = 0;
            //dropdownListing.Clear();
        }
        public DoorKeyComponentData(DoorKeyComponentData data)
        {
            IsActive = data.IsActive;
            isDoor = data.isDoor;
            isKey = data.isKey;
            doorValue = data.doorValue;
            keyValue = data.keyValue;
            selectedOption = data.selectedOption;
            extraFields = data.extraFields;
        }
    }
    [System.Serializable]
    public class ChestKeyComponentData
    {
        public bool IsActive;
        public static List<string> chestDropdownListing = new List<string>();
        public bool isKey;
        public bool isChest;
        public List<string> chestValue = new List<string>();
        public string keyValue = "";
        public List<int> _selectedChestOption = new List<int>();
        public int extraFields = 0;
        public List<TMPro.TMP_Dropdown> _extraValues = new List<TMPro.TMP_Dropdown>();

        public ChestKeyComponentData()
        {
            IsActive = false;
            isChest = false;
            isKey = false;
            // chestValue = "";
            keyValue = "";
            chestValue.Clear();
            //   selectedOption = 0;
            _selectedChestOption.Clear();
            //  dropdownListing.Clear();
        }
        public void Reset()
        {
            IsActive = false;
            isChest = false;
            isKey = false;
            //chestValue = "";
            chestValue.Clear();
            keyValue = "";
            //selectedOption = 0;
            _selectedChestOption.Clear();// = 0;
            //dropdownListing.Clear();
        }
        public ChestKeyComponentData(ChestKeyComponentData data)
        {
            IsActive = data.IsActive;
            isChest = data.isChest;
            isKey = data.isKey;
            chestValue = data.chestValue;
            keyValue = data.keyValue;
            _selectedChestOption = data._selectedChestOption;
            extraFields = data.extraFields;
        }
    }
    [System.Serializable]
    public class ElapsedTimeComponentData
    {
        public bool IsActive;
        public bool IsStart;
        public bool IsEnd;
        // public float Timer;

        public ElapsedTimeComponentData()
        {
            IsActive = false;
            IsStart = false;
            IsEnd = false;
            //    Timer = 0f;
        }
        public void Reset()
        {
            IsStart = false;
            IsEnd = false;
            IsActive = false;
            //   Timer = 0f;
        }
        public ElapsedTimeComponentData(ElapsedTimeComponentData data)
        {
            IsActive = data.IsActive;
            IsEnd = data.IsEnd;
            IsStart = data.IsStart;
        }
    }

    [System.Serializable]
    public class HealthComponentData
    {
        public bool IsActive;
        public float points;

        public HealthComponentData()
        {
            IsActive = false;
            points = 0f;
        }

        public void Reset()
        {
            IsActive = false;
            points = 0f;
        }

        public HealthComponentData(HealthComponentData data)
        {
            IsActive = data.IsActive;
            points = data.points;
        }
    }

    [System.Serializable]
    public class PowerProviderComponentData
    {
        public bool IsActive;
        public float setTimer;
        public float playerSpeed;
        public float playerHeight;

        public PowerProviderComponentData()
        {
            IsActive = false;
            setTimer = 10;
            playerSpeed = 12;
            playerHeight = 8;
        }

        public void Reset()
        {
            IsActive = false;
            setTimer = 10;
            playerSpeed = 12;
            playerHeight = 8;
        }

        public PowerProviderComponentData(PowerProviderComponentData data)
        {
            IsActive = data.IsActive;
            setTimer = data.setTimer;
            playerSpeed = data.playerSpeed;
            playerHeight = data.playerHeight;
        }
    }

    [System.Serializable]
    public class AvatarChangerComponentData
    {
        public bool IsActive;
        public float setTimer;
        public int avatarIndex;

        public AvatarChangerComponentData()
        {
            IsActive = false;
            setTimer = 5;
            avatarIndex = 0;
        }

        public void Reset()
        {
            IsActive = false;
            setTimer = 5;
            avatarIndex = 0;
        }
        public AvatarChangerComponentData(AvatarChangerComponentData data)
        {
            IsActive = data.IsActive;
            setTimer = data.setTimer;
            avatarIndex = data.avatarIndex;
        }
    }

    [System.Serializable]
    public class ThrowThingsComponentData
    {
        public bool IsActive;
        public float force;
        public ThrowThingsComponentData()
        {
            IsActive = false;
            force = 3;
        }

        public void Reset()
        {
            IsActive = false;
            force = 3;
        }
        public ThrowThingsComponentData(ThrowThingsComponentData data)
        {
            IsActive = data.IsActive;
            force = data.force;
        }
    }
    [System.Serializable]
    public class SituationChangerComponentData
    {
        public bool IsActive;
        public float Timer;

        public SituationChangerComponentData()
        {
            IsActive = false;
            Timer = 0f;
        }

        public void Reset()
        {
            IsActive = false;
            Timer = 0f;
        }

        public SituationChangerComponentData(SituationChangerComponentData data)
        {
            IsActive = data.IsActive;
            Timer = data.Timer;
        }
    }

    [System.Serializable]
    public class SpecialItemComponentData
    {
        public bool IsActive;
        public float setTimer;
        public float playerSpeed;
        public float playerHeight;

        public SpecialItemComponentData()
        {
            IsActive = false;
            setTimer = 10;
            playerSpeed = 2;
            playerHeight = 1.5f;
        }

        public void Reset()
        {
            IsActive = false;
            setTimer = 10;
            playerSpeed = 2;
            playerHeight = 1.5f;
        }

        public SpecialItemComponentData(SpecialItemComponentData data)
        {
            IsActive = data.IsActive;
            setTimer = data.setTimer;
            playerSpeed = data.playerSpeed;
            playerHeight = data.playerHeight;
        }
    }
    [System.Serializable]
    public class NinjaComponentData
    {
        public bool IsActive;
        public float ninjaSpeed;
        public float setTimerNinjaEffect;

        public NinjaComponentData()
        {
            IsActive = false;
            ninjaSpeed = 1f;
            setTimerNinjaEffect = 15;
        }

        public void Reset()
        {
            IsActive = false;
            ninjaSpeed = 1f;
            setTimerNinjaEffect = 15;
        }

        public NinjaComponentData(NinjaComponentData data)
        {
            IsActive = data.IsActive;
            ninjaSpeed = data.ninjaSpeed;
            setTimerNinjaEffect = data.setTimerNinjaEffect;
        }
    }

    [System.Serializable]
    public class SpawnerItemComponentData
    {
        public bool IsActive;


        public SpawnerItemComponentData()
        {
            IsActive = false;

        }

        public void Reset()
        {
            IsActive = false;

        }

        public SpawnerItemComponentData(SpawnerItemComponentData data)
        {
            IsActive = data.IsActive;
        }
    }

    [System.Serializable]
    public class DisplayMessageComponentData
    {
        public bool IsActive;
        public string titleDisplayMessage;
        public float messageDisplayTimeData;
        public List<string> displayMessageData;
        public List<TMPro.TMP_InputField> informationDataRewriting;

        public DisplayMessageComponentData()
        {
            IsActive = false;
            titleDisplayMessage = "Enter Title";
            messageDisplayTimeData = 5f;
            displayMessageData = new List<string>();
            informationDataRewriting = new List<TMPro.TMP_InputField>();
        }
        public void Reset()
        {
            IsActive = false;
            titleDisplayMessage = "Enter Title";
            messageDisplayTimeData = 5f;
            displayMessageData = new List<string>();
            informationDataRewriting = new List<TMPro.TMP_InputField>();
        }
        public DisplayMessageComponentData(DisplayMessageComponentData data)
        {
            IsActive = data.IsActive;
            titleDisplayMessage = data.titleDisplayMessage;
            messageDisplayTimeData = data.messageDisplayTimeData;
            displayMessageData = data.displayMessageData;
            informationDataRewriting = data.informationDataRewriting;
        }
    }

    [System.Serializable]
    public class QuizComponentData
    {
        public bool IsActive;
        public List<TMPro.TMP_InputField> rewritingInputList;
        public List<string> rewritingStringList;
        public List<int> answers;

        public QuizComponentData()
        {
            IsActive = false;
            rewritingInputList = new List<TMPro.TMP_InputField>();
            rewritingStringList = new List<string>();
            answers = new List<int>();

        }
        public void Reset()
        {
            IsActive = false;
            rewritingInputList = new List<TMPro.TMP_InputField>();
            rewritingStringList = new List<string>();
            answers = new List<int>();

        }
        public QuizComponentData(QuizComponentData data)
        {
            IsActive = data.IsActive;
            rewritingInputList = data.rewritingInputList;
            rewritingStringList = data.rewritingStringList;
            answers = data.answers;

        }
    }


    #endregion
}