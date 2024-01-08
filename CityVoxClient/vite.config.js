import { defineConfig } from 'vite'
import viteBasicSslPlugin from '@vitejs/plugin-basic-ssl'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react() , viteBasicSslPlugin()],
  
})
