import GameObject from "./game-object.js";

export default class Footprint extends GameObject {
	/**
	 * @param {number} x
	 * @param {number} y
	 */
	constructor(x, y) {
		super(x, y);
	}

	render() {
		const element = document.createElement('div');
		element.classList.add('footprint');
		return element;
	}
}
