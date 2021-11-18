// Author: Thomas Campbell
// Project: B.E.V.I.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vanity {

    /// <summary>
    /// Class that produces random names for lakes, rivers, roads, and other features.
    /// </summary>
    public class NameFactory
    {
        public static string[] rdPf = new string[] {"Road", "Street", "Alley", "Avenue", "Circle", "Causeway", "Boulevard", "Highway", "Lane", "Plaza", "Way", "Drive", "Bypass"};
        public static string[] rdNames = new string[] {"Campbell", "Nelson", "Spillman", "Broken", "Chestnut", "Moonset", "Elephant", "Thomas", "January", "Aaron", "King James", "Black Brick", "Treefall", "Beverly Hills", "Fallen Knight",
                                                "Port", "Bent Tree", "Amberly", "Virginia Beach", "Sunray", "Fabulous", "Triptych", "Gateway", "Sunrise Kingdom", "Reapvale", "Brokenland", "Thomas Campbell", "January Nelson", "Aaron Spillman",
                                                "Pendulum", "Butterfly", "Ulysses S. Grant", "Andrew Garfield", "Iron Mountian", "Lonely Mountian", "Ruby", "Whiterun", "Deckard", "Apple Blossom", "George Custer", "Abernethy", "Clearfield",
                                                "River", "Columbia", "London Bridge", "Lee", "Edgewood", "Wyoming", "Cataract", "Ivy", "Bellevue", "Prospect", "Inscryption", "Cole", "Winchester", "Remington", "Shotgun", "Vermont", "Morningside",
                                                "Heron", "Andrew Ryan", "Rick Deckard", "Black Rock", "Main", "Smithsonian", "Overgrowth", "Midanola", "Arrakis", "Coalmine", "Murky Water", "Goodybag", "Tranquil", "Oxpond", "Trader", "Blueberry",
                                                "Raspberry", "Neeman", "Rick Rolling", "Vanderbelt", "Garriott", "Memorial", "Rememberance", "Upgrades People", "Phineas T. Ratchet", "Benjamin Kaber", "Lars Lohrsword", "Arthur Graywell",
                                                "Lovecraft", "Star of the North", "Volk Vendleberg", "Linnaeus", "Serena", "Rotisserie Chicken"};
        public static string[] lakePf = new string[] {"Lake", "Cove", "Pond", "Depths", "Reservoir", "Lagoon"};
        public static string[] lakeNames = new string[] {"Clearwater", "Vast", "GrandPort", "Murkydeath", "Loss", "Waterplains", "Mayflower", "Gopalan", "Arctic", "Moon-Lit", "Surging", "Ford", "Pilgrim", "Goo", "Bikini Bottom", "Quietscream", "Crocodile",
                                                "Yellow", "Tripfall", "Spinner", "Honor", "Careers", "Shipwreck", "Erie", "Huron", "Icedam", "Knight", "Blue Peter", "Higginbottom", "Right Arm", "Bowmaker", "Kessah", "Dadislav", "Gambino", "Death",
                                                "Campbell", "Spillman", "Nelson"};
        public static string[] riverPf = new string[] {"River", "Brook", "Canal", "Creek", "Run", "Beck", "Channel", "Tributary", "Stream"};
        public static string[] riverNames = new string[] {"Campbell", "Nelson", "Spillman", "Thomas", "Aaron", "January", "James", "Mississippi", "United States", "England", "Mordor", "Violent", "Rolling", "Rushing", "Tadple", "Ships End", "Hungry",
                                                    "Moaning", "Dartmoth", "George Mason", "George Washington", "Abraham Lincoln", "Crouse", "Wainwright", "Buhner", "Drunkard's", "Giraffe", "Diamond", "Emerald", "Styx", "Broken Heart",
                                                    "Sanguine", "Nausica", "Nixon", "JFK", "Truman", "Churchill", "Darkwood", "Stormcloak", "Silver", "Axehalt", "Rounded Stone", "Mountian's Break", "Ocean's End", "Crying Sky", "Melted Snow",
                                                    "Homework", "Latenight", "Sleeping Giant", "Endless", "Waterfall", "Raven's Point", "Moody", "Muddy", "Grace's Fall", "Sarah Grace"};

        public static string genRoad() {
            string roadName = rdNames[Random.Range(0, rdNames.Length)];
            string postfix = "";
            if(roadName == "Rick Rolling") {
                postfix = "Road";
            }
            else {
                postfix = rdPf[Random.Range(0, rdPf.Length)];
            }

            return (roadName + " " + postfix);
        }

        public static string genLake() {
            string lakeName = lakeNames[Random.Range(0, lakeNames.Length)];
            string postfix = "";
            postfix = lakePf[Random.Range(0, lakePf.Length)];

            return (lakeName + " " + postfix);
        }

        public static string genRiver() {
            string riverName = riverNames[Random.Range(0, riverNames.Length)];
            string postfix = "";
            postfix = riverPf[Random.Range(0, riverPf.Length)];

            return (riverName + " " + postfix);
        }

        public static string genCreek() {
            string creekName = riverNames[Random.Range(0, riverNames.Length)];
            string postfix = "Creek";

            return (creekName + " " + postfix);
        }
    }
}