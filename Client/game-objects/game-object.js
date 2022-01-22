
export default class GameObject {

	/**
	 * @param {number} x
	 * @param {number} y
	 */
	constructor(x, y) {
		this.#checkValueIsPositiveNumber(x);
		this.#checkValueIsPositiveNumber(y);

		/**
		 * @type {number}
		 */
		this.x = x;

		/**
		 * @type {number}
		 */
		this.y = y;
	}

	/**
	 * @param {number} value
	 */
	#checkValueIsPositiveNumber(value) {
		if (isNaN(value) || value < 0) {
			throw new Error('x cannot be less than zero');
		}
	}

	render() { }
}


