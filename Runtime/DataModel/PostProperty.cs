using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BennyKok.NotionAPI.Post
{
    [Serializable]
    public class Parent
    {
        public string type;
        public string database_id;
    }

    [Serializable]
    public class OptionEntry
    {
        //public string id;
        public string name;
        //public string color;
    }

    [Serializable]
    public class SelectProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public OptionEntry select;
    }

    [Serializable]
    public class MultiSelectProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public OptionEntry[] multi_select;
    }

    [Serializable]
    public class TitleProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public Text[] title;
    }

    [Serializable]
    public class TextProperty
    {
        //public string id;
        //public string type;
        public Text[] rich_text;
    }

    [Serializable]
    public class Text
    {
        //public string type;
        public TextContent text;
        public Annotations annotations = new Annotations();
        //public string plain_text;
        //public object href;

        [Serializable]
        public class TextContent
        {
            public string content;
            public object link;
        }

        [Serializable]
        public class Annotations
        {
            public bool bold;
            public bool italic;
            public bool strikethrough;
            public bool underline;
            public bool code;
            public string color = "default";
        }
    }

    [Serializable]
    public class NumberProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public float number;
    }

    [Serializable]
    public class CheckboxProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public bool checkbox;
    }

    [Serializable]
    public class DateProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public Date date;
    }

    [Serializable]
    public class Date
    {
        public string start;
        public string end;
    }

    [Serializable]
    public class EmailProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public string email;
    }

    [Serializable]
    public class PhoneNumberProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public string phone_number;
    }

    [Serializable]
    public class UrlProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public string url;
    }

    [Serializable]
    public class PersonProperty
    {
        //public string id;
        //public string name;
        //public string type;
        public People[] people;
    }

    [Serializable]
    public class People
    {
        public string @object;
        public string id;
    }
}