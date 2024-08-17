pipeline {
    agent any 

    environment {
        // Adiciona o caminho para ferramentas .NET globais ao PATH
        PATH = "$PATH:$HOME/.dotnet/tools"
    }

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
        stage('Executar Testes e Coletar Cobertura') {
            steps {
                script {
                    // Executa os testes e coleta cobertura de código
                    sh "dotnet test --collect:\"XPlat Code Coverage\""
                }
            }
        }
        stage('SonarQube Analysis') {
            steps {
                script {
                    withSonarQubeEnv('SonarQube Server') {
                        sh "dotnet sonarscanner begin /k:\"RGR-TRANSPORTE\" /d:sonar.host.url=\"http://66.135.11.124:9000\" /d:sonar.cs.opencover.reportsPaths=\"**/TestResults/*/coverage.opencover.xml\""
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
