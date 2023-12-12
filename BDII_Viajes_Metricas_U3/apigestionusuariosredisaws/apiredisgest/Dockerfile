FROM python:3.8

WORKDIR /app

COPY requirements.txt requirements.txt
RUN pip install -r requirements.txt

COPY . .

CMD ["gunicorn", "-k", "gevent", "-w", "1", "-b", "0.0.0.0:5000", "app.routes:app_dispatch"]
#CMD ["gunicorn", "-w", "1", "-b", "0.0.0.0:5000", "--timeout", "120", "--keep-alive", "10", "app.routes:app_dispatch"]
#CMD ["python", "./app/routes.py"]
