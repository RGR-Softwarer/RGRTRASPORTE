pipeline {
    agent any 

    environment {
        PATH = "$PATH:$HOME/.dotnet/tools"
    }

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
        stage('Construir e Subir Serviços') {
            steps {
                script {
                    sh "docker-compose up -d --build"
                }
            }
        }
        stage('Executar Testes e Coletar Cobertura') {
            steps {
                script {
                    sh "dotnet test --collect:\"XPlat Code Coverage\" --logger trx --results-directory ./TestResults"
                }
            }
        }
        stage('SonarQube Analysis') {
            steps {
                script {
                    withSonarQubeEnv('SonarQube Server') {
                        sh "dotnet sonarscanner begin /k:\"RGR-TRANSPORTE\" /d:sonar.host.url=\"http://66.135.11.124:9000\" /d:sonar.cs.opencover.reportsPaths=\"TestResults/coverage.cobertura.xml\" /d:sonar.cs.vstest.reportsPaths=\"TestResults/*.trx\" /d:sonar.scanner.scanAll=false"
                        sh "dotnet build"
                        sh "dotnet sonarscanner end"
                    }
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
