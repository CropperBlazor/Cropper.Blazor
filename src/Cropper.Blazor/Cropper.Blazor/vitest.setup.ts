import { defineConfig } from 'vitest/config';

export default defineConfig({
    test: {
        globals: true,
        environment: 'jsdom',
        include: ['**/*.{test,spec}.{ts,tsx,js,jsx}'],
        isolate: true,
        coverage: {
            provider: 'v8',
            enabled: true,
            reportsDirectory: './coverage',
            reporter: ['text', 'lcov', 'cobertura'],
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
