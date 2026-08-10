import { defineConfig } from "vitest/config";

export default defineConfig({
  esbuild: {
    sourcemap: "inline",
  },
  test: {
    globals: true,
    environment: "jsdom",
    include: ["Cropper/**/*.coverage.test.ts"],
    fileParallelism: false,
    isolate: false,
    pool: "forks",
    coverage: {
      provider: "istanbul",
      enabled: false,
      reportsDirectory: "./coverage",
      reporter: ["text", "html", "cobertura", "json"],
      all: true,
      include: [
        "Cropper/cropperCanvasInteropCommands.ts",
        "Cropper/cropperDataInteropCommands.ts",
        "Cropper/cropperImageInteropCommands.ts",
        "Cropper/cropperSelectionInteropCommands.ts",
        "Cropper/cropperViewerInteropCommands.ts",
        "Cropper/helpers/cropper-url-image-helper.ts",
        "Cropper/helpers/blob-helper.ts",
      ],
      exclude: [
        "**/*.d.ts",
        "**/*.spec.*",
        "**/*.test.*",
        "Cropper/**/*.types.ts",
        "Cropper/types/**",
        "node_modules/**",
        "dist/**",
      ],
    },
  },
});
