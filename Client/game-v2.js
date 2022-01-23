import ApiClient from "./api-client.js"
import CookieHelper from "./cookie-helper.js";

export default class GameV2 {
	constructor() {
		this.#client = new ApiClient();
	}

	/**
	 * @type {ApiClient}
	 */
	#client = null;

	async init() {
		const playerId = CookieHelper.getCookie("playerId");
		if(playerId) {
			await this.#client.connect(playerId);
		}
	}

	async start() {
		const playerId = await this.#client.startNewGame();
		CookieHelper.setCookie("playerId", playerId);
	}

	async restart() {
		const playerId = await this.#client.restartGame();
		CookieHelper.setCookie("playerId", playerId);
	}
}



