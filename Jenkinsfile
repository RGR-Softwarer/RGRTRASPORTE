pipeline {
    agent any 

    stages {
        stage('Checkout') {
            steps {
                script {
                    // Se você tiver configurado o pipeline como "Pipeline script from SCM" ou "Multibranch Pipeline"
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
        stage('Construir e Subir Serviços') {
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
        stage('SonarQube Analysis') {
            steps {
                script {
                    withSonarQubeEnv('SonarQube Server') {
                        sh "dotnet sonarscanner begin /k:\"RGR-TRANSPORTE\" /d:sonar.host.url=\"http://66.135.11.124:9000\""
                        sh "dotnet build"
                        sh "dotnet sonarscanner end"
                    }
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
