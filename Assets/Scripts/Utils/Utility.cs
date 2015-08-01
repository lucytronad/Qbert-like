using UnityEngine;
using System;
using System.Collections;
using System.Xml;

public static class Utility {

    public static int ParseStringToInt(string value)
    {
        int number;
        try
        {
            number = Convert.ToInt32(value);
            return number;
        }
        catch (FormatException e)
        {
            Debug.LogError("Input string is not a sequence of digits.");
            throw e;
        }
        catch (OverflowException e)
        {
            Debug.LogError("The number cannot fit in an Int32.");
            throw e;
        }
    }

    public static float ParseStringToFloat(string value)
    {
        float number;
        try
        {
            number = float.Parse(value);
            return number;
        }
        catch(FormatException e)
        {
            Debug.LogError("Input string is not valid.");
            throw e;
        }
        catch(OverflowException e)
        {
            Debug.LogError("The number cannot fit in a float.");
            throw e;
        }
    }

    public static Vector3 GetNodePosition(XmlNode node)
    {
         return new Vector3(ParseStringToFloat(node.Attributes["x"].Value),
                            ParseStringToFloat(node.Attributes["y"].Value),
                            ParseStringToFloat(node.Attributes["z"].Value));
    }

}    

