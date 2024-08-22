pipeline {
    agent any 

    stages {
        stage('Checkout') {
            steps {
                script {
                    checkout scm 
                }
            }
        }
        stage('Parar Serviços') {
            steps {
                script {
                    sh "docker-compose down"
                }
            }
        }
        stage('Instalar Dependências') {
            steps {
                script {
                    sh "npm install"
                }
            }
        }
        stage('Análise SonarQube') {
            steps {
                script {
                    withSonarQubeEnv('SonarQube Server') {
                        sh "sonar-scanner \
                            -Dsonar.projectKey=RGRFRONT \
                            -Dsonar.sources=. \
                            -Dsonar.host.url=http://66.135.11.124:9000"
                    }
                }
            }
        }
        stage('Construir e Subir Serviços com Docker Compose') {
            steps {
                script {
                    sh "docker-compose up -d --build"
                }
            }
        }
        stage('Limpar Imagens Docker') {
            steps {
                script {
                    sh "docker image prune -f"
                }
            }
        }
        stage('Limpar Recursos Docker') {
            steps {
                script {
                    sh "docker network prune -f"
                    sh "docker volume prune -f"
                }
            }
        }
    }
    post {
        always {
            cleanWs()
        }
    }
}
