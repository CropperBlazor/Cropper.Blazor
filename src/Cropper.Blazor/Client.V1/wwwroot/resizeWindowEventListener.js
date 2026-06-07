let timer
window.addEventListener('resize', () => {
  if (!Object.hasOwn(this, 'cropper') || cropper == null || cropper.cropperInstances == null) { // eslint-disable-line no-undef
    return
  }
  const keys = Object.keys(cropper.cropperInstances) // eslint-disable-line no-undef
  clearTimeout(timer)
  if (keys.length > 0) {
    keys.forEach((key) => {
      cropper.cropperInstances[key].disable() // eslint-disable-line no-undef
    })
    timer = setTimeout(() => {
      const keys = Object.keys(cropper.cropperInstances) // eslint-disable-line no-undef
      keys.forEach((key) => {
        cropper.cropperInstances[key].enable() // eslint-disable-line no-undef
      })
    }, 100)
  }
})
