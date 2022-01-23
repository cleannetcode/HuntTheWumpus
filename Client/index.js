import GameV2 from "./game-v2.js";
import Game from "./game.js";

const app = new Game();
app.run();

const gameV2 = new GameV2();
gameV2.init();
document.app = gameV2;
