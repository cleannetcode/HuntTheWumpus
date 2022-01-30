import Footprint from "./game-objects/footprint.js";
import GameMap from "./game-objects/game-map.js";
import Player from "./game-objects/player.js";
import ApiClient from "./api-client.js";

export default class Game {
	/**
	 * @param {ApiClient} client
	 * @param {number} mapSize
	 */
	constructor(client, mapSize) {
		this.#client = client;
		this.#player = new Player(1, 1);
		this.map = new GameMap([this.#player], mapSize);
	}
	#steps = 0;
	#shoots = 0;
	/**
	 * @type {Player}
	 */
	#player = null;
	/**
	 * @type {ApiClient}
	 */
	#client = null;

	/**
	 * @param {Direction} direction
	 * @returns
	 */
	async move(direction) {
		if (!this.#player.isAlive) {
			return;
		}

		var result = await this.#client.playerMove(direction);

		console.log(result);
	}

	/**
	 * @param {Direction} direction
	 * @returns
	 */
	async attack(direction) {
		if (!this.#player.isAlive) {
			return;
		}
	}

	/**
	 * @param {number} x
	 * @param {number} y
	 */
	#placeFootprint(x, y) {
		const room = this.map.getRoom(x, y);
		const oldFootprint = room.getObject(x => x instanceof Footprint);
		if (!oldFootprint) {
			room.add(new Footprint(x, y));
		}
	}
}
