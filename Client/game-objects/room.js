import GameObject from "./game-object.js";

export default class Room {
	/**
	 * @param {GameObject} gameObject
	 */
	constructor(gameObject) {
		if (gameObject && gameObject instanceof GameObject == false) {
			throw new Error('Room can be created only with instance of GameObject.');
		}

		if (gameObject) {
			this.#gameObjects.push(gameObject);
		}
	}

	/**
	 * @type {GameObject[]}
	 */
	#gameObjects = [];

	/**
	 * @param {GameObject} gameObject
	 */
	add(gameObject) {
		if (!gameObject || gameObject instanceof GameObject == false) {
			throw new Error('gameObject cannot be null and should be instance of GameObject.');
		}

		this.#gameObjects.push(gameObject);
	}

	/**
	 * @param {GameObject} gameObject
	 */
	remove(gameObject) {
		if (!gameObject || gameObject instanceof GameObject == false) {
			throw new Error('gameObject cannot be null and should be instance of GameObject.');
		}

		const index = this.#gameObjects.indexOf(gameObject);
		this.#gameObjects.splice(index, 1);
	}

	getObjects() {
		return new Array(...this.#gameObjects);
	}

	/**
	 * @callback predicate
	 * @param {GameObject} gameObject
	 * @returns {boolean}
	 *
	 * @param {predicate} predicate - The callback that handles the response.
	 */
	getObject(predicate) {
		if (!predicate) {
			throw new Error('predicat cannot be null or undefined');
		}

		return this.#gameObjects.find(predicate);
	}

	render() {
		const roomElement = document.createElement('div');
		roomElement.classList.add('room');

		if (this.#gameObjects && this.#gameObjects.length > 0) {

			for (const gameObject of this.#gameObjects) {
				const objectElement = gameObject.render();
				if (objectElement) {
					roomElement.append(objectElement);
				}
			}
		}

		return roomElement;
	}
}
