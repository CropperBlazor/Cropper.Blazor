window.downloadFromUrl = (options) => {
    const anchorElement = document.createElement('a');
    anchorElement.href = options.url;
    anchorElement.download = options.fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
};

window.addClipPathPolygon = (sourceCanvas, path) => {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    const width = sourceCanvas.width,
        height = sourceCanvas.height;

    canvas.width = width;
    canvas.height = height;
    context.imageSmoothingEnabled = true;

    context.beginPath();
    context.moveTo(path[0] * width / 100, path[1] * height / 100);
    context.fillStyle = "rgba(255, 255, 255, 0)";

    for (let i = 2; i < path.length; i += 2) {
        context.lineTo(path[i] * width / 100, path[i + 1] * height / 100);
    }

    context.closePath();
    context.clip();
    context.fill();
    context.globalCompositeOperation = 'lighter';
    context.drawImage(sourceCanvas, 0, 0, width, height);

    return canvas.toDataURL();
}

window.addClipPathCircle = (sourceCanvas) => {
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');
    const width = sourceCanvas.width,
        height = sourceCanvas.height;

    canvas.width = width;
    canvas.height = height;
    context.imageSmoothingEnabled = true;
    context.drawImage(sourceCanvas, 0, 0, width, height);
    context.globalCompositeOperation = 'destination-in';
    context.beginPath();
    context.ellipse(width / 2, height / 2, width / 2, height / 2, 0 * Math.PI, 0, 180 * Math.PI, true);
    context.fill();

    return canvas.toDataURL();
}
