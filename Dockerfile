# Usar a imagem oficial do Node.js como base para construir o projeto
FROM node:20-alpine AS build

# Configurar o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copiar os arquivos de configuração do Node.js e as dependências
COPY package.json package-lock.json ./

# Instalar as dependências do projeto
RUN npm install

# Copiar todo o código-fonte do projeto
COPY . .

# Construir o projeto para produção (se aplicável)
RUN npm run build

# Usar uma imagem NGINX oficial para servir o aplicativo
FROM nginx:alpine

# Copiar a build do projeto para o diretório de servimento do NGINX
COPY --from=build /app/dist/rgrtrasporte /usr/share/nginx/html

# Copiar a configuração personalizada do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expor a porta 80 do contêiner
EXPOSE 80

# Comando para iniciar o NGINX e servir a aplicação
CMD ["nginx", "-g", "daemon off;"]
