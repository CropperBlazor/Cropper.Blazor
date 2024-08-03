window.downloadFromUrl = (options) => {
  const anchorElement = document.createElement("a");
  anchorElement.href = options.url;
  anchorElement.download = options.fileName ?? "";
  anchorElement.click();
  anchorElement.remove();
};

window.getPolygonImage = (sourceCanvas, path) => {
  const canvas = document.createElement("canvas");
  const context = canvas.getContext("2d");
  const width = sourceCanvas.width,
    height = sourceCanvas.height;

  canvas.width = width;
  canvas.height = height;
  context.imageSmoothingEnabled = true;

  context.beginPath();
  context.moveTo((path[0] * width) / 100, (path[1] * height) / 100);
  context.fillStyle = "rgba(255, 255, 255, 0)";

  for (let i = 2; i < path.length; i += 2) {
    context.lineTo((path[i] * width) / 100, (path[i + 1] * height) / 100);
  }

  context.closePath();
  context.clip();
  context.fill();
  context.globalCompositeOperation = "lighter";
  context.drawImage(sourceCanvas, 0, 0, width, height);

  return canvas.toDataURL("image/png", 1);
};

window.getEllipseImage = (sourceCanvas) => {
  const createdCanvas = document.createElement("canvas");
  const contextCanvas = createdCanvas.getContext("2d");
  const widthCanvas = sourceCanvas.width,
    heightCanvas = sourceCanvas.height;

  createdCanvas.width = widthCanvas;
  createdCanvas.height = heightCanvas;
  contextCanvas.imageSmoothingEnabled = true;

  contextCanvas.drawImage(sourceCanvas, 0, 0, widthCanvas, heightCanvas);
  contextCanvas.globalCompositeOperation = "destination-in";
  contextCanvas.beginPath();
  contextCanvas.ellipse(
    widthCanvas / 2,
    heightCanvas / 2,
    widthCanvas / 2,
    heightCanvas / 2,
    0 * Math.PI,
    0,
    180 * Math.PI,
    true,
  );
  contextCanvas.fill();

  return createdCanvas.toDataURL("image/png", 1);
};
