# skaper en ny local git repositroy ska göras vid nya projekt och göras 1 gång per projekt
git init

# lägg till en github remote repositroy för lagring i molnet , detta görs bara en gång per projekt
git remote add origin https://github.com/saifyousifsson/saif.git



# ligger till nya filer/ändrade filer till min local git respository göras varje gång du gjort ändringar
git add .

# spara alla ändringar till din local git repositroy , göras varje gång du gjort ändrringar
git commit -m "lektion 1"

# skickas upp alla filer till remote git repositroy , göras varje gång du görjt ändringer
git push origin main






#för att hämta hem de senaste från din remote git repositroy , köras värje gång du vill hämta hem nya saker 
git pull origin main

# för att hämta min remote git repositroy från min github , göras bara en gång 
git clone https://github.com/saifyousifsson/saif.git
