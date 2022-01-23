export default class ApiClient {

	#baseUrl = "https://localhost:7030";

	/**
	 * @returns {Promise<string>} playerId
	 */
	async startNewGame() {
		const response = await fetch(`${this.#baseUrl}/Game/newGame`, {
			method: 'POST'
		});

		if (!response.ok) {
			var error = await response.text();
			throw new Error(error);
		}

		const json = await response.json();
		return json.playerId;
	}

	/**
	 * @returns {Promise<string>} playerId
	 */
	async restartGame() {
		const response = await fetch(`${this.#baseUrl}/Game/newGame`, {
			method: 'PUT'
		});

		if (!response.ok) {
			var error = await response.text();
			throw new Error(error);
		}

		const json = await response.json();
		return json.playerId;
	}

	/**
	 * @param {string} playerId
	 * @returns
	 */
	 async connect(playerId) {
		const response = await fetch(`${this.#baseUrl}/Game/connection`, {
			method: 'POST',
			headers: {
				"Content-Type": "application/json"
			},
			body: JSON.stringify({ playerId: playerId })
		});

		if (!response.ok) {
			var error = await response.text();
			throw new Error(error);
		}
	}
}

