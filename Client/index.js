import Direction from "./Direction.js";
import GameMap from "./game-objects/game-map.js";
import Player from "./game-objects/player.js";
import Pit from "./game-objects/pit.js";
import Bats from "./game-objects/bats.js";
import Wumpus from "./game-objects/wumpus.js";
import MoveableObject from "./game-objects/moveable-object.js";
import Footprint from "./game-objects/footprint.js";

class Game {
	#steps = 0;
	#shoots = 0;

	run() {
		const controlsElement = document.getElementById('controls');
		const movementElement = this.#renderMovementControls();
		const attackElement = this.#renderAttackControls();
		controlsElement.prepend(movementElement);
		controlsElement.append(attackElement);

		this.#setupKeyControls();

		this.#start();
		this.#updateGameInfo();
		this.#draw();
	}

	/**
	 * @param {Direction} direction
	 * @returns
	 */
	move(direction) {
		if (!this.player.isAlive) {
			return;
		}

		const x = this.player.x;
		const y = this.player.y;
		this.#moveGameObject(this.player, direction);
		this.#placeFootprint(x, y);
		this.#steps++;

		this.#update();
		this.#redraw();
	}

	/**
	 * @param {Direction} direction
	 * @returns
	 */
	attack(direction) {
		if (!this.player.isAlive) {
			return;
		}

		const arrow = this.player.shoot(direction);

		if (arrow.x == this.wumpus.x && arrow.y == this.wumpus.y) {
			this.wumpus.die();
		}

		this.#shoots++;

		this.#update();
		this.#redraw();
	}

	/**
	 * @param {number} x
	 * @param {number} y
	 */
	#placeFootprint(x, y) {
		const room = this.map.getRoom(x, y);
		const oldFootprint = room.getObject(x => x instanceof Footprint);
		if(!oldFootprint) {
			room.add(new Footprint(x, y));
		}
	}

	#start() {
		const size = 5;

		let possibleCoordinates = [];
		let coordinate = {x:0, y:0};

		for (let x = 0; x < size; x++) {
			for (let y = 0; y < size; y++) {
				possibleCoordinates.push({x, y});
			}
		}

		({ coordinate, possibleCoordinates } = this.#getRandomCoordinate(possibleCoordinates));
		this.player = new Player(coordinate.x, coordinate.y);

		({ coordinate, possibleCoordinates } = this.#getRandomCoordinate(possibleCoordinates));
		this.wumpus = new Wumpus(coordinate.x, coordinate.y);

		const gameObjects = [];
		gameObjects.push(this.player);
		gameObjects.push(this.wumpus);

		for (let i = 0; i < 2; i++) {
			({ coordinate, possibleCoordinates } = this.#getRandomCoordinate(possibleCoordinates))
			gameObjects.push(new Pit(coordinate.x, coordinate.y));
			({ coordinate, possibleCoordinates } = this.#getRandomCoordinate(possibleCoordinates))
			gameObjects.push(new Bats(coordinate.x, coordinate.y));
		}

		this.map = new GameMap(gameObjects, size);
	}

	/**
	 * @param {{x:0, y:0}[]} possibleCoordinates
	 */
	#getRandomCoordinate(possibleCoordinates) {
		const indexOfCoordinate = Math.round(Math.random() * (possibleCoordinates.length - 1));
		const coordinate = possibleCoordinates[indexOfCoordinate];
		possibleCoordinates = possibleCoordinates.filter((_, index) => index != indexOfCoordinate);
		return { coordinate, possibleCoordinates };
	}

	#restart() {
		this.#start();
		this.#updateGameInfo();
		this.#redraw();
	}

	/**
	 * @param {MoveableObject} moveableObject
	 * @param {Direction} direction
	 * @returns
	 */
	#moveGameObject(moveableObject, direction) {
		if (!direction) {
			throw Error('direction cannot be null or undefined');
		}

		if (!moveableObject || !moveableObject instanceof MoveableObject) {
			throw Error('only MoveableObject can be moved by Game.moveGameObject');
		}

		if (!this.map.isValidDirection(moveableObject, direction)) {
			return;
		}

		let room = this.map.getRoom(moveableObject.x, moveableObject.y);
		room.remove(moveableObject);

		const {x, y} = moveableObject.move(direction);

		room = this.map.getRoom(x, y);
		room.add(moveableObject);
	}

	#update() {
		if (!this.player.isAlive) {
			const result = confirm("Ты проиграл!\nПопробуешь еще раз?");

			if (result) {
				this.#restart()
			}

			return;
		}

		if (!this.wumpus.isAlive) {
			const result = confirm("Ты победил!\nПопробуешь еще раз?");

			if (result) {
				this.#restart()
			}

			return;
		}

		const roomWithPlayer = this.map.getRoom(this.player.x, this.player.y);

		const wumpus = roomWithPlayer.getObject(x => x instanceof Wumpus);
		const pit = roomWithPlayer.getObject(x => x instanceof Pit);
		const bats = roomWithPlayer.getObject(x => x instanceof Bats);

		if (wumpus || pit) {
			this.player.die();
			this.#update();
		} else if (bats) {
			const x = Math.floor(Math.random() * this.map.size);
			const y = Math.floor(Math.random() * this.map.size);

			let room = this.map.getRoom(this.player.x, this.player.y);
			room.remove(this.player);

			this.player.x = x;
			this.player.y = y;

			room = this.map.getRoom(this.player.x, this.player.y);
			room.add(this.player);
			this.#update();
		} else {
			const isWumpusSleep = Math.round(Math.random());
			if (!isWumpusSleep) {
				this.#moveGameObject(this.wumpus, Direction.random);
				const roomWithWumpus = this.map.getRoom(this.wumpus.x, this.wumpus.y);
				const player = roomWithWumpus.getObject(x => x instanceof Player);
				if (player) {
					this.player.die();
					this.#update();
				}
			}
		}

		this.#updateGameInfo();
	}

	#redraw() {
		this.#clean();
		this.#draw();
	}

	#draw() {
		// const gameElement = document.getElementById('game');
		const mapElement = this.map.render();
		// gameElement.prepend(mapElement);

		const stepsElement = document.getElementById("steps");
		stepsElement.innerText = this.#steps;

		const shootsElement = document.getElementById("shoots");
		shootsElement.innerText = this.#shoots;
	}

	#updateGameInfo() {
		let gameObjects = [];

		for (let x = this.player.x - 1; x <= this.player.x + 1; x++) {
			for (let y = this.player.y - 1; y <= this.player.y + 1; y++) {

				if (x == this.player.x && y == this.player.y) {
					continue;
				}

				if (x < 0 || y < 0 || x >= this.map.size || y >= this.map.size) {
					continue;
				}

				const room = this.map.getRoom(x, y);
				gameObjects.push(...room.getObjects());
			}
		}

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
		movementButton.onclick = () => this.move(direction);
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
		}

		window.onkeyup = (e) => {
			if (e.defaultPrevented) {
				return;
			}

			switch (e.code) {
				case "ArrowUp":
					this.move(Direction.up);
					break;
				case "ArrowDown":
					this.move(Direction.down);
					break;
				case "ArrowRight":
					this.move(Direction.right);
					break;
				case "ArrowLeft":
					this.move(Direction.left);
					break;

				case "KeyW":
					this.attack(Direction.up);
					break;
				case "KeyA":
					this.attack(Direction.left);
					break;
				case "KeyD":
					this.attack(Direction.right);
					break;
				case "KeyS":
					this.attack(Direction.down);
					break;

				default:
					break;
			}

			e.preventDefault();
		}
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
		attackButton.onclick = () => this.attack(direction);
		attackButton.innerText = name;
		return attackButton;
	}
}

const app = new Game();
app.run();

document.app = app;
