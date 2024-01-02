using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System.IO;

namespace Map.Events
{
    /// <summary>
    /// Apply Vertical Layout Group and Content Size fitter to Content for this to work as itnended
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        [SerializeField] private MapEvent curEvent;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI body;
        [SerializeField] private Transform content;

        [Header("Prefabs")]
        [SerializeField] private ActionButton actionButtonPrefab;


        private void Start()
        {
            Init();
        }


        [ContextMenu("Do Init")]
        public void Init()  {
            List<MapEvent> events = GetEvents();

            curEvent = events[Random.Range(0, events.Count)];

            DrawSlide(curEvent, curEvent.InitialSlide);
        }

        private List<MapEvent> GetEvents() {
            List<MapEvent> result = new List<MapEvent>();

            string path = Application.dataPath + "/Events/";
            DirectoryInfo eventDirectory = new DirectoryInfo(path);
            DirectoryInfo[] eventFolders = eventDirectory.GetDirectories();

            foreach (var mapEvent in eventFolders)
            {
                FileInfo[] files = mapEvent.GetFiles();
                foreach (var file in files) {
                    if (file.FullName.EndsWith(".event"))
                    { // regex equivalent : .*\.event$
                        using (StreamReader sr = file.OpenText()) {
                            result.Add(MapEvent.FromJson(sr.ReadToEnd()));
                        }
                    }
                }
            }

            return result;
        }

        private void DrawSlide(MapEvent mapEvent, string slideID) {
            MapEventSlide slide = mapEvent.Slides.Where(cur => cur.SlideID == slideID).FirstOrDefault();
            if (slide == null) {
                Debug.LogError("Didnt find matching slide");
                return;
            }

            body.text = slide.Text;

            foreach (var item in slide.mapEventActions)
                AddActionButton(item);
            

        }

        private void AddActionButton(MapEventAction action)
        {
            ActionButton newButton = Instantiate(actionButtonPrefab, content.transform);

            newButton.ChangeButtonText(action.ActionText);
            newButton.callback.AddListener(() => DoActionLogic(action.ActionLogics));
        }
        private void DoActionLogic(List<MapEventActionLogic> logic) {
            Debug.Log($"Did action with logic {logic}");
        }
    }

}

