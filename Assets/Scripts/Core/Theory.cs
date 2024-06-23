using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theory :  Information
{
    public static List<Theory> TheoryList = new List<Theory>();

    private int _id = 0;
    private string _description = "";

    public int ID
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    public string Description
    {
        get
        {
            return _description;
        }
        set
        {
            _description = value;
        }
    }

}
