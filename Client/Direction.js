export default class Direction {
	static get up() { return 1; };
	static get down() { return 2; };
	static get right() { return 3; };
	static get left() { return 4; };

	static get random() { return Math.floor(Math.random() * 4) + 1; }
}
