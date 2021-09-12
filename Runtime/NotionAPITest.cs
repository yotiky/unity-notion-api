using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace BennyKok.NotionAPI
{
    public class NotionAPITest : MonoBehaviour
    {
        public string apiKey;
        public string database_id;

        private IEnumerator Start()
        {
            var api = new NotionAPI(apiKey);

            yield return api.GetDatabase<CardDatabasePropertiesDefinition>(database_id, (db) =>
            {
                Debug.Log(db.id);
                Debug.Log(db.created_time);
                Debug.Log(db.title.First().text.content);

                Debug.Log(JsonUtility.ToJson(db));
            });

#if !UNITY_2020_1_OR_NEWER
            yield return api.QueryDatabase<CardDatabaseQueryResponse>(database_id, (db) =>
            {
                Debug.Log(JsonUtility.ToJson(db));
            });
#else
            yield return api.QueryDatabase<CardDatabaseProperties>(database_id, (db) =>
            {
                Debug.Log(JsonUtility.ToJson(db));
            });
#endif
        }

        [Serializable]
        public class CardDatabasePropertiesDefinition
        {
            public MultiSelectPropertyDefinition Tags;
            public TitleProperty Name;
            public CheckboxProperty IsActive;
            public DateProperty Date;
            public SelectPropertyDefinition Type;
            public NumberProperty number;
            public TextProperty Description;
        }

        [Serializable]
        public class CardDatabaseProperties
        {
            public MultiSelectProperty Tags;
            public TitleProperty Name;
            public CheckboxProperty IsActive;
            public DateProperty Date;
            public SelectProperty Type;
            public NumberProperty number;
            public NumberProperty UseTime;
            public TextProperty Description;
        }

        [Serializable]
        public class CardDatabaseQueryResponse
        {
            public CardDatabasebPage[] results;
        }

        [Serializable]
        public class CardDatabasebPage : Page<CardDatabaseProperties> { }
    }
}