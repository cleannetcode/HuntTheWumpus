namespace HuntTheWumpus.API.Domain.GameObjects
{
    public class Room
    {
        private List<GameObject> _gameObjects;
        public Room()
        {
            _gameObjects = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        public void Remove(GameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
        }

        public GameObject[] GetObjects()
        {
            return _gameObjects.ToArray();
        }

        public GameObject? GetObject(Func<GameObject, bool> predicate)
        {
            return _gameObjects.FirstOrDefault(predicate);
        }
    }
}
