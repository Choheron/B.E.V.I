/// <summary>
/// Enums - January Nelson, Aaron Spillman, Thomas Campbell
/// Project: B.E.V.I.
/// Object Enum Pools
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This enum is expandable for further features in the future.
/// </summary>
public enum FeatureType{ river = 1, lake, street, stamp, building, region, poi }
public enum StampType{ streetSign = 1, lampPost, lampPole, vendingMachine, bench, hangingLight, hangingSign, trashBin, recycleBin, billboard1, billboard2, tableWChairs, tree1, gardenEmpty, gardenGrowing, thomasPerson, aaronPerson, janPerson}
public enum MapDirection{down = 0, left = 90, up = 180, right = 270 }
public enum DetailType{ StrtEdge = 1, StrtArea, WtrEdge, WtrArea, Building}
public enum MaterialType{ Grass = 1, Dirt, Gravel, Concrete, Brush, Sand, Cobble, Wood }
public enum SpriteAtlases{ MaterialsAtlas = 1, DetailsAtlas, StampsAtlas, EditorAtlas, MetaAtlas }
public enum MetaType{ Region = 1, Pin }
public enum EditorType{ Clear = 1, GlowBadV1, GlowGoodV1 }