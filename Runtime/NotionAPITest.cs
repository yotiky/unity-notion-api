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

            var param = new CardDatabasePostRequest
            {
                parent = new Post.Parent { database_id = database_id },
                properties = new CardDatabasePostProperties
                {
                    Name = new Post.TitleProperty
                    {
                        title = new[] { new Post.Text { text = new Post.Text.TextContent { content = "Attack1" } } }
                    },
                    Tags = new Post.MultiSelectProperty
                    {
                        multi_select = new[] { new Post.OptionEntry { name = "SomeTag" } }
                    },
                    Description = new Post.TextProperty
                    {
                        rich_text = new[] {
                            new Post.Text { 
                                text = new Post.Text.TextContent { content = "Sample data.", },
                                annotations = new Post.Text.Annotations
                                {
                                    bold = true, color = "green", code = true, italic = true, strikethrough = true, underline = true,
                                }
                        },
                    }
                    },
                    UseTime = new Post.NumberProperty
                    {
                        number = 100,
                    },
                    Type = new Post.SelectProperty
                    {
                        select = new Post.OptionEntry { name = "SomeAttack" },
                    },
                    Date = new Post.DateProperty
                    {
                        date = new Post.Date { start = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"), end = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") },
                    },
                    Person = new Post.PersonProperty
                    {
                        people = new[] {
                        new Post.People { @object = "user", id = "valid id" }
                    }
                    },
                    IsActive = new Post.CheckboxProperty
                    {
                        checkbox = true,
                    },
                    Url = new Post.UrlProperty
                    {
                        url = "https://www.hogehoge.com",
                    },
                    Email = new Post.EmailProperty
                    {
                        email = "hoge@email.com",
                    },
                    Phone = new Post.PhoneNumberProperty
                    {
                        phone_number = "012344445555",
                    },
                }
            };

            var json = JsonUtility.ToJson(param);
            Debug.Log(json);

            yield return api.CreatePage(param, (res) =>
            {
                Debug.Log(res);
            });
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

        [Serializable]
        public class CardDatabasePostRequest
        {
            public Post.Parent parent;
            public CardDatabasePostProperties properties;
        }

        [Serializable]
        public class CardDatabasePostProperties
        {
            public Post.MultiSelectProperty Tags;
            public Post.TitleProperty Name;
            public Post.CheckboxProperty IsActive;
            public Post.DateProperty Date;
            public Post.SelectProperty Type;
            public Post.NumberProperty UseTime;
            public Post.TextProperty Description;
            public Post.UrlProperty Url;
            public Post.EmailProperty Email;
            public Post.PhoneNumberProperty Phone;
            public Post.PersonProperty Person;
        }
    }
}