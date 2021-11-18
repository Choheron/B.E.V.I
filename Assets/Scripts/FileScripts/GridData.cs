///<author> Aaron Spillman </author>
///<project> BEVI </project>
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
[System.Serializable]
public class GridData
{
    
    public int width;
    public int height;
    //int[] materials;
    //int[][] tids;
   // int[][] orientations;
    private MapTable mapTable;
    private List<List<Feature>> features;
    private GD_OVERSEER OH;

    public const int  BEVI_GOD_DIVIDE = 42069420;

    public GridData(GridMap grid){
        OH = new GD_OVERSEER();
        OH.width = grid.width;
        OH.height = grid.height;
        OH.squareData = new GD_SD[grid.width * grid.height];
        int squareDataCount = 0;
        List<string> t = new List<string>();
        mapTable = grid.mt;
        mapTable.GetMasterTable( out features, out t);
        
        for(int w = 0; w < grid.width; w++){
            
            for(int h = 0; h < grid.height; h++){
                GD_SD sdtemp = new GD_SD();
                int[] thisTriagonalSign = new int[4]; 
                int fsinchat;
                grid.data[w,h].SaveMaterial(out fsinchat, out thisTriagonalSign);
                Debug.Log("fsinchat: " + fsinchat);
                sdtemp.mtrl = fsinchat;
                sdtemp.tils = thisTriagonalSign;
                Debug.Log(sdtemp.mtrl);
                OH.squareData[squareDataCount] = sdtemp;
                squareDataCount++;
            }
        }
        List<GD_SF> kool = new List<GD_SF>();
        FeatureType mything;
        for(int i = 0; i < features.Count; i++){
            mything = (FeatureType)(i + 1);
            for(int j = 0; j < features[i].Count; j++){
                Feature temp = features[i][j];
                GD_SF sftemp = new GD_SF();
                sftemp.ftt = (int)mything;
                switch(mything){
                    case FeatureType.street:
                    case FeatureType.river:
                        FLine liner = (FLine)temp;
                        sftemp.coords = new int[]{liner.x1, liner.y1, liner.x2, liner.y2};
                        break;
                    case FeatureType.lake:
                    case FeatureType.building:
                    case FeatureType.region:
                        FArea arear = (FArea)temp;
                        sftemp.coords = new int[]{arear.x1, arear.y1, arear.x2, arear.y2};
                        break;
                    case FeatureType.stamp:
                    case FeatureType.poi:
                        FDot dotter = (FDot)temp;
                        sftemp.coords = new int[]{dotter.x, dotter.y, -1, (int)dotter.rot};
                        if(mything == FeatureType.stamp){
                            Stamp stemp = (Stamp)temp;
                            sftemp.coords[2] = (int)stemp.stType;
                        }
                        break;
                }
                sftemp.tn = temp.contents.topName;
                sftemp.clr = new float[]{temp.color.r, temp.color.b, temp.color.g, temp.color.a};
                sftemp.ifk = new string[temp.contents.fields.Count];
                sftemp.dispsterl = new int[temp.contents.fields.Count];
                sftemp.ifi = new string[temp.contents.fields.Count];
                int oo = 0;
                foreach(InfoField q in temp.contents.fields){
                    sftemp.ifk[oo] = q.fieldName;
                    sftemp.dispsterl[oo] = q.displayStyle;
                    sftemp.ifi[oo] = q.input.ToString();
                    oo++;
                }
                kool.Add(sftemp);
            }
            OH.featureData = kool;
        }
        Debug.Log("I think it's saved!");
        //OH SHOULD BE FINISHED
    }
    public string SaveToString(){
        Debug.Log(Application.persistentDataPath);
        return TranslateToJson();
    }

    public string TranslateToJson(){
        string aBigString = JsonUtility.ToJson(OH, false);
       /* foreach(GD_SD[] d1 in OH.squareData){
            foreach(GD_SD d2 in d1){
                aBigString += JsonUtility.ToJson(d2, true);
            }
        }*/
        /*foreach(GD_SD poggers in OH.squareData){
            aBigString += JsonUtility.ToJson(poggers, false);
        }*/
        Debug.Log(aBigString);
        return aBigString;
    }

    

}

[System.Serializable]
public class GD_OVERSEER{
    public int width, height;
    public GD_SD[] squareData;
    public List<GD_SF> featureData;
}

[System.Serializable]
public class GD_SD{
    public int mtrl;//material type
    public int[] tils;//tile tids
}
[System.Serializable]
public class GD_SF{
    public int ftt;//FEATURE TYPE
    public int[] coords;//coordinates
    public float[] clr;//color
    public string tn;//top name
    public string[] ifk;//Field names
    public int[] dispsterl;
    public string[] ifi;//Field inputs
}