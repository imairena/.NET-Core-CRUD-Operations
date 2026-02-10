pipeline {
    agent any

    environment {
        // Path to your solution file
        SOLUTION_PATH = 'StudentAPI.sln'
    }

    stages {
        stage('Checkout') {
            steps {
                // Checkout code from your repository
                // 'scm' refers to the Source Control Management configured in the Jenkins job
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                echo 'Restoring NuGet packages...'
                // Restores dependencies defined in the solution file
                sh "dotnet restore ${SOLUTION_PATH}"
            }
        }

        stage('Build') {
            steps {
                echo 'Building solution...'
                // Builds the project in Release configuration without restoring again
                sh "dotnet build ${SOLUTION_PATH} --configuration Release --no-restore"
            }
        }

        stage('Test') {
            steps {
                echo 'Running Unit Tests...'
                // Executes all tests in the solution. 
                // --no-build ensures we test exactly what we just built.
                sh "dotnet test ${SOLUTION_PATH} --no-build --configuration Release --verbosity normal"
            }
        }
    }
}
