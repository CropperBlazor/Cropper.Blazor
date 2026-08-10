import { defineConfig } from 'vitest/config';
export default defineConfig({
    esbuild: {
        sourcemap: 'inline'
    },
    test: {
        globals: true,
        environment: 'jsdom',
        include: ['Cropper/**/*.{test,spec}.{ts,tsx,js,jsx}'],
        fileParallelism: false,
        isolate: false,
        pool: 'forks',
        coverage: {
            provider: 'istanbul',
            enabled: false,
            reportsDirectory: '../Cropper.Blazor.Tests/coverage',
            reporter: ['text', 'html', 'cobertura'],
            allowExternal: true,
            all: true,
            include: [
                'Cropper/helpers/cropper-url-image-helper.ts',
                'Cropper/helpers/blob-helper.ts'
            ],
            exclude: [
                '**/*.d.ts',
                '**/*.spec.*',
                '**/*.test.*',
                'node_modules/**',
                'dist/**'
            ]
        }
    }
});
