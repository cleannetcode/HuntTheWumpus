import MoveableObject from "./moveable-object.js";

export default class Wumpus extends MoveableObject {
	/**
	 * @param {number} x
	 * @param {number} y
	 */
	constructor(x, y) {
		super(x, y);

		this.#isAlive = true;
	}

	#isAlive = false;

	get isAlive() {
		return this.#isAlive;
	}

	die() {
		this.#isAlive = false;
	}
}
