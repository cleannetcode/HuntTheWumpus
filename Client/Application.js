import ApiClient from "./api-client.js";
import CookieHelper from "./cookie-helper.js";
import Direction from "./Direction.js";
import Pit from "./game-objects/pit.js";
import Bats from "./game-objects/bats.js";
import Wumpus from "./game-objects/wumpus.js";
import Game from "./GameV2.js";


export default class Application {
	constructor() {
		this.#client = new ApiClient();
	}

	/**
	 * @type {ApiClient}
	 */
	#client = null;

	/**
	 * @type {Game}
	 */
	#game = null;

	async init() {
		const controlsElement = document.getElementById('controls');
		const movementElement = this.#renderMovementControls();
		const attackElement = this.#renderAttackControls();
		controlsElement.prepend(movementElement);
		controlsElement.append(attackElement);
		this.#setupKeyControls();

		const playerId = CookieHelper.getCookie("playerId");
		if (playerId) {
			await this.#client.connect(playerId);
		}
	}

	async start() {
		// new Game
		const gameState = await this.#client.startNewGame();
		CookieHelper.setCookie("playerId", gameState.playerId);
		this.#game = new Game(this.#client, gameState.mapSize);
		this.#draw();
	}

	async restart() {
		// new Game
		const playerId = await this.#client.restartGame();
		CookieHelper.setCookie("playerId", playerId);
		this.#game = new Game(this.#client, 5);
		this.#draw();
	}

	#redraw() {
		this.#clean();
		this.#draw();
	}

	#draw() {
		this.#game.map.render();

		const stepsElement = document.getElementById("steps");
		stepsElement.innerText = this.#game.steps;

		const shootsElement = document.getElementById("shoots");
		shootsElement.innerText = this.#game.shoots;
	}


	#updateGameInfo() {

		const wumpusInfoElement = document.getElementById('wumpus-info');
		const batsInfoElement = document.getElementById('bats-info');
		const pitInfoElement = document.getElementById('pit-info');

		pitInfoElement.className = "hide";
		batsInfoElement.className = "hide";
		wumpusInfoElement.className = "hide";

		for (const gameObject of gameObjects) {
			if (gameObject instanceof Pit) {
				pitInfoElement.className = "";
			}

			if (gameObject instanceof Bats) {
				batsInfoElement.className = "";
			}

			if (gameObject instanceof Wumpus) {
				wumpusInfoElement.className = "";
			}
		}
	}

	#clean() {
		const mapElement = document.getElementById('map');
		if (mapElement) {
			mapElement.innerHTML = "";
		}
	}

	#renderMovementControls() {
		const movementElement = document.createElement('div');
		movementElement.classList.add("control-keys");

		const moveUpElement = this.#createMovementButton('Up', Direction.up);
		moveUpElement.classList.add("up");
		const moveDownElement = this.#createMovementButton('Down', Direction.down);
		moveDownElement.classList.add("down");
		const moveLeftElement = this.#createMovementButton('Left', Direction.left);
		moveLeftElement.classList.add("left");
		const moveRightElement = this.#createMovementButton('Right', Direction.right);
		moveRightElement.classList.add("right");

		const nameMovementElement = document.createElement('p');
		nameMovementElement.innerText = 'Премещение';

		movementElement.append(nameMovementElement);
		movementElement.append(moveLeftElement, moveDownElement, moveUpElement, moveRightElement);

		return movementElement;
	}

	/**
	 * @param {string} name
	 * @param {Direction} direction
	 * @returns
	 */
	#createMovementButton(name, direction) {
		const movementButton = document.createElement('button');
		movementButton.onclick = () => this.#game.move(direction);
		movementButton.innerText = name;
		return movementButton;
	}

	#setupKeyControls() {

		window.onkeydown = (e) => {
			switch (e.code) {
				case "ArrowUp":
				case "ArrowDown":
				case "ArrowRight":
				case "ArrowLeft":
					e.preventDefault();
					break;

				default:
					break;
			}
		};

		window.onkeyup = (e) => {
			if (e.defaultPrevented) {
				return;
			}

			switch (e.code) {
				case "ArrowUp":
					this.#game.move(Direction.up);
					break;
				case "ArrowDown":
					this.#game.move(Direction.down);
					break;
				case "ArrowRight":
					this.#game.move(Direction.right);
					break;
				case "ArrowLeft":
					this.#game.move(Direction.left);
					break;

				case "KeyW":
					this.#game.attack(Direction.up);
					break;
				case "KeyA":
					this.#game.attack(Direction.left);
					break;
				case "KeyD":
					this.#game.attack(Direction.right);
					break;
				case "KeyS":
					this.#game.attack(Direction.down);
					break;

				default:
					break;
			}

			e.preventDefault();
		};
	}

	#renderAttackControls() {
		const attackElement = document.createElement('div');
		attackElement.classList.add("control-keys");

		const attackUpButton = this.#createAttackButton('Up', Direction.up);
		attackUpButton.classList.add("up");
		const attackDownButton = this.#createAttackButton('Down', Direction.down);
		attackDownButton.classList.add("down");
		const attackLeftButton = this.#createAttackButton('Left', Direction.left);
		attackLeftButton.classList.add("left");
		const attackRightButton = this.#createAttackButton('Right', Direction.right);
		attackRightButton.classList.add("right");

		const nameAttackElement = document.createElement('p');
		nameAttackElement.innerText = 'Стрельба';

		attackElement.append(nameAttackElement);
		attackElement.append(attackLeftButton, attackDownButton, attackUpButton, attackRightButton);

		return attackElement;
	}

	#createAttackButton(name, direction) {
		const attackButton = document.createElement('button');
		attackButton.onclick = () => this.#game.attack(direction);
		attackButton.innerText = name;
		return attackButton;
	}
}
