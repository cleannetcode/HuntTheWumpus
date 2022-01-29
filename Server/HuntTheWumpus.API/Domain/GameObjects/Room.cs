namespace HuntTheWumpus.API.Domain.GameObjects
{
    public class Room
    {
        private List<GameObject> _gameObjects;
        public Room() { }
        public Room(GameObject gameObject)
        {
            _gameObjects = new List<GameObject> { gameObject };
        }

        public void Add(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        public void Remove(GameObject gameObject)
        {
            throw new NotImplementedException();
            //_gameObjects.Remove(gameObject)
        }

        public GameObject[] getObjects()
        {
            return _gameObjects.ToArray();
        }

        public GameObject? GetObject(Func<GameObject, bool> predicate)
        {
            return _gameObjects.FirstOrDefault(predicate);
	}
    }
}
