pipeline {
    agent any 

    stages {
        stage('Checkout') {
            steps {
                checkout scm 
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
                    def scannerHome = tool name: 'SonarScanner for .NET', type: 'hudson.plugins.sonar.SonarRunnerInstallation'
                    withSonarQubeEnv('SonarQube Server') {
                        sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll begin /k:\"RGR-TRANSPORTE\""
                        sh "dotnet build"
                        sh "dotnet ${scannerHome}/SonarScanner.MSBuild.dll end"
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
