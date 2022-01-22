import MoveableObject from "./moveable-object.js";
import Direction from "../Direction.js";

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

	/**
	 * @param {Direction} direction
	 * @returns
	 */
	attack(direction) {
		const attackRange = 1;

		let x = 0;
		let y = 0;

		switch (direction) {
			case Direction.up:
				x = this.x;
				y = this.y - attackRange
				break;

			case Direction.down:
				x = this.x;
				y = this.y + attackRange
				break;

			case Direction.left:
				x = this.x - attackRange;
				y = this.y;
				break;

			case Direction.right:
				x = this.x + attackRange;
				y = this.y;
				break;

			default:
				break;
		}

		return new Arrow(x, y);
	}
}
