const { defineConfig, globalIgnores } = require('eslint/config');
const tsParser = require('@typescript-eslint/parser');
const tsPlugin = require('@typescript-eslint/eslint-plugin');
const prettierPlugin = require('eslint-plugin-prettier');

module.exports = defineConfig([
    globalIgnores(['**/node_modules/**']),

    {
        files: ['Cropper/**/*.{ts,tsx,mts,cts}'],

        languageOptions: {
            parser: tsParser,
            parserOptions: {
                ecmaVersion: 'latest',
                sourceType: 'module',
            },
        },

        plugins: {
            '@typescript-eslint': tsPlugin,
            prettier: prettierPlugin,
        },

        rules: {
            // TypeScript rules
            '@typescript-eslint/no-unused-vars': [
                'error',
                { argsIgnorePattern: '^_' },
            ],

            // Prettier integration
            'prettier/prettier': 'error',
        },
    },
]);
