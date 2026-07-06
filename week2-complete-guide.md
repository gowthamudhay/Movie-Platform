# Week 2 — Complete Guide
## Dockerize → Push to AWS → Deploy to ECS

> Do these in exact order. Don't skip ahead.

---

## STEP 1 — Confirm Docker is working

Open VS Code terminal:

```powershell
docker --version
```

You should see something like `Docker version 24.x.x`. If this fails, Docker Desktop isn't open — open it from the Start menu and wait for the whale icon in your taskbar to stop animating (that means it's ready).

---

## STEP 2 — Create the backend Dockerfile

In VS Code, right click `backend/MovieApi` folder → **New File** → name it exactly:

```
Dockerfile
```

(no extension, capital D)

Paste this content:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MovieApi.dll"]
```

---

## STEP 3 — Create the frontend Dockerfile

Right click `frontend/movie-frontend` folder → **New File** → name it:

```
Dockerfile
```

Paste this:

```dockerfile
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json .
RUN npm install
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
```

In the same folder, create another file called:

```
nginx.conf
```

Paste this:

```nginx
server {
    listen 80;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }
}
```

---

## STEP 4 — Update the frontend API URL (make it flexible)

Open `frontend/movie-frontend/src/services/api.js` and replace with:

```javascript
import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api'
})

export const movieService = {
  getAll: () => api.get('/movies'),
  getById: (id) => api.get(`/movies/${id}`),
  search: (q) => api.get(`/movies/search?q=${q}`),
  getGenres: () => api.get('/movies/genres')
}
```

---

## STEP 5 — Create docker-compose.yml (test both containers together)

In your root folder `D:\movie-app`, create a new file:

```
docker-compose.yml
```

Paste this:

```yaml
version: '3.8'

services:
  movie-api:
    build:
      context: ./backend/MovieApi
    ports:
      - "5000:8080"

  movie-frontend:
    build:
      context: ./frontend/movie-frontend
    ports:
      - "3000:80"
    depends_on:
      - movie-api
```

---

## STEP 6 — Test Docker locally

Close any terminals running `dotnet run` or `npm run dev` first (press `Ctrl + C` in each).

In VS Code terminal:

```powershell
cd D:\movie-app
docker-compose up --build
```

This takes a few minutes the first time — it's downloading base images.

Wait until you see both services say they're listening. Then open browser:

```
http://localhost:3000
```

You should see your movie app, now running fully through Docker.

**If this works, send me a screenshot — then we move to AWS. If you get an error, paste the exact error text.**

---

## STEP 7 — Set up AWS account and CLI

### 7.1 Create AWS account (skip if you already have one)

Go to **aws.amazon.com** → Create a free account. You'll need a card for verification (won't be charged if you stay in free tier).

### 7.2 Install AWS CLI

Download from: **aws.amazon.com/cli** (Windows installer)

After installing, restart VS Code, then check:

```powershell
aws --version
```

### 7.3 Create an IAM user (don't use your root account)

1. Go to AWS Console → search **IAM**
2. Click **Users** → **Create user**
3. Name: `movie-platform-admin`
4. Tick **"Provide user access to the AWS Management Console"** — optional
5. Click **Next** → **Attach policies directly**
6. Search and tick these policies:
   - `AmazonECS_FullAccess`
   - `AmazonEC2ContainerRegistryFullAccess`
   - `AmazonS3FullAccess`
   - `CloudWatchLogsFullAccess`
   - `IAMFullAccess`
7. Click **Create user**

### 7.4 Create access keys

1. Click on the user you just created
2. Go to **Security credentials** tab
3. Click **Create access key**
4. Choose **Command Line Interface (CLI)**
5. Click through and **Create access key**
6. **Copy both the Access Key ID and Secret Access Key** — you won't see the secret again

### 7.5 Configure AWS CLI

```powershell
aws configure
```

It will ask 4 things:
```
AWS Access Key ID: (paste it)
AWS Secret Access Key: (paste it)
Default region name: eu-west-1
Default output format: json
```

Test it worked:

```powershell
aws sts get-caller-identity
```

You should see your account ID and username in JSON.

---

## STEP 8 — Create S3 bucket for assets

```powershell
aws s3 mb s3://movie-platform-assets-YOURNAME --region eu-west-1
```

Replace `YOURNAME` with something unique (your name, random numbers, etc — bucket names must be globally unique across all of AWS).

Confirm it was created:

```powershell
aws s3 ls
```

---

## STEP 9 — Create ECR repositories (Docker image storage)

```powershell
aws ecr create-repository --repository-name movie-api --region eu-west-1
aws ecr create-repository --repository-name movie-frontend --region eu-west-1
```

**Copy the `repositoryUri` value from each output** — you'll need it in the next step. It looks like:
```
123456789012.dkr.ecr.eu-west-1.amazonaws.com/movie-api
```

---

## STEP 10 — Push your Docker images to ECR

### 10.1 Authenticate Docker with ECR

Replace `YOUR_ACCOUNT_ID` with your actual 12-digit AWS account number (find it by running `aws sts get-caller-identity`):

```powershell
aws ecr get-login-password --region eu-west-1 | docker login --username AWS --password-stdin YOUR_ACCOUNT_ID.dkr.ecr.eu-west-1.amazonaws.com
```

You should see `Login Succeeded`.

### 10.2 Build and push the API image

```powershell
cd D:\movie-app\backend\MovieApi
docker build -t movie-api .
docker tag movie-api:latest YOUR_ACCOUNT_ID.dkr.ecr.eu-west-1.amazonaws.com/movie-api:latest
docker push YOUR_ACCOUNT_ID.dkr.ecr.eu-west-1.amazonaws.com/movie-api:latest
```

### 10.3 Build and push the frontend image

```powershell
cd D:\movie-app\frontend\movie-frontend
docker build -t movie-frontend .
docker tag movie-frontend:latest YOUR_ACCOUNT_ID.dkr.ecr.eu-west-1.amazonaws.com/movie-frontend:latest
docker push YOUR_ACCOUNT_ID.dkr.ecr.eu-west-1.amazonaws.com/movie-frontend:latest
```

---

## STEP 11 — Create the ECS cluster (via AWS Console — easier first time)

1. Go to AWS Console → search **ECS**
2. Click **Create cluster**
3. Cluster name: `movie-platform`
4. Infrastructure: select **AWS Fargate (serverless)**
5. Click **Create**

---

## STEP 12 — Create task definitions

### 12.1 API task definition

1. In ECS Console → left sidebar → **Task definitions** → **Create new task definition**
2. Task definition family: `movie-api-task`
3. Launch type: **AWS Fargate**
4. CPU: `0.25 vCPU`, Memory: `0.5 GB`
5. Container details:
   - Name: `movie-api`
   - Image URI: `YOUR_ACCOUNT_ID.dkr.ecr.eu-west-1.amazonaws.com/movie-api:latest`
   - Container port: `8080`
6. Click **Create**

### 12.2 Frontend task definition

Repeat the same steps:
1. Task definition family: `movie-frontend-task`
2. Container name: `movie-frontend`
3. Image URI: `YOUR_ACCOUNT_ID.dkr.ecr.eu-west-1.amazonaws.com/movie-frontend:latest`
4. Container port: `80`
5. Click **Create**

---

## STEP 13 — Create ECS services (this actually runs your containers)

### 13.1 API service

1. Go to your `movie-platform` cluster → **Create service**
2. Launch type: **Fargate**
3. Task definition: `movie-api-task`
4. Service name: `movie-api-service`
5. Desired tasks: `1`
6. Networking: choose your default VPC, select all public subnets
7. Security group: **Create new** → allow inbound on port `8080` from anywhere (0.0.0.0/0)
8. Load balancing: **Create new Application Load Balancer**
   - Name: `movie-api-lb`
   - Listener port: `80`
   - Target port: `8080`
9. Click **Create**

### 13.2 Frontend service

Repeat:
1. Task definition: `movie-frontend-task`
2. Service name: `movie-frontend-service`
3. Security group: allow inbound on port `80`
4. Load balancing: **Create new Application Load Balancer**
   - Name: `movie-frontend-lb`
   - Listener port: `80`
   - Target port: `80`
5. Click **Create**

---

## STEP 14 — Wait and verify

Services take 2-5 minutes to start. Check status:

```powershell
aws ecs describe-services --cluster movie-platform --services movie-api-service movie-frontend-service --query "services[].{name:serviceName,running:runningCount,desired:desiredCount}"
```

Both should show `running: 1, desired: 1`.

---

## STEP 15 — Get your live URL

1. In AWS Console, search **EC2** → left sidebar → **Load Balancers**
2. Click on `movie-frontend-lb`
3. Copy the **DNS name** (looks like `movie-frontend-lb-123456789.eu-west-1.elb.amazonaws.com`)
4. Paste that into your browser

**Your app is now live on the internet.** ✅

---

## STEP 16 — Final checklist

- [ ] Docker images built and tested locally
- [ ] Images pushed to ECR successfully
- [ ] ECS cluster created
- [ ] Both task definitions created
- [ ] Both services running (runningCount: 1)
- [ ] Load balancer DNS name opens the app in browser
- [ ] S3 bucket created for assets
- [ ] IAM user created with correct permissions
- [ ] Screenshot taken of live app
- [ ] README updated with live URL
- [ ] Code pushed to GitHub

---

## When something breaks

Send me:
1. The exact step number you're on
2. The exact error message (copy-paste, not paraphrased)
3. A screenshot if it's visual

Don't try to guess-fix AWS errors yourself — paste them here first, AWS error messages are usually very specific about what's wrong.
