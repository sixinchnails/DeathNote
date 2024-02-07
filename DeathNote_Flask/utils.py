import string, random

def generate_song_title(pattern, length=22):
    """
    Generates a random song title based on a given pattern.

    :param pattern: The pattern to use as a prefix for the title (e.g., '0' for titles starting with '0').
    :param length: The total length of the song ID (excluding the '.wav' extension).
    :return: A randomly generated song title.
    """
    # Generate a random string of uppercase letters and digits
    # Subtract the length of the pattern and the length of the extension (.wav = 4 characters)
    random_part = ''.join(random.choices(string.ascii_uppercase + string.digits, k=length - len(pattern) - 4))

    # Concatenate the pattern, the random part, and the '.wav' extension
    title = f"{pattern}{random_part}"

    return title

def suggest_song_title(features):
    themes = {
        'acousticness': [
            'Nature', 'Earth', 'Acoustic', 'Wooden', 'Organic', 'Bare', 'Serene', 'Natural', 'Mellow', 'Unplugged',
            'Whisper', 'Resonance', 'Echo', 'Pure', 'Herbal', 'Folklore', 'Raw', 'Primitive', 'Harmony', 'Reverberate',
            'Silent', 'Calm', 'Peaceful', 'Tradition', 'Simplicity', 'Earthy', 'Airy', 'Rustic', 'Pastoral', 'Soothing'
        ],
        'danceability': [
            'Groove', 'Dance', 'Move', 'Rhythm', 'Flow', 'Swing', 'Bounce', 'Hop', 'Step', 'Shake',
            'Jump', 'Twist', 'Jive', 'Rock', 'Sway', 'Spin', 'Slide', 'Boogie', 'Funk', 'Glide',
            'Disco', 'Swirl', 'Vibe', 'Groovy', 'Twirl', 'Step-up', 'Waltz', 'Hustle', 'Bump', 'Flex'
        ],
        'energy': [
            'Power', 'Energetic', 'Force', 'Intensity', 'Dynamic', 'Explosive', 'Potent', 'Vigorous', 'Robust',
            'Mighty',
            'Impact', 'Electric', 'Vitality', 'Thriving', 'Zeal', 'Vivid', 'Fervent', 'Fierce', 'Passionate', 'Kinetic',
            'Radiant', 'Strong', 'Vibrant', 'Active', 'Charged', 'Lively', 'Ardent', 'Hearty', 'Rousing', 'Stirring'
        ],
        'instrumentalness': [
            'Orchestra', 'Strings', 'Symphony', 'Instrumental', 'Harmony', 'Ensemble', 'Sonata', 'Rhapsody', 'Concerto',
            'Interlude',
            'Sonnet', 'Nocturne', 'Opera', 'Virtuoso', 'Serenade', 'Overture', 'Melody', 'Fugue', 'Quartet', 'Chamber',
            'Cadenza', 'Minuet', 'Requiem', 'Capriccio', 'Concertino', 'Aria', 'Divertimento', 'Prelude', 'Etude',
            'Finale'
        ],
        'liveness': [
            'Live', 'Concert', 'Stage', 'Crowd', 'Audience', 'Performance', 'Showtime', 'Applause', 'Encore', 'Gig',
            'Festival', 'Venue', 'Arena', 'Unison', 'Act', 'Session', 'Tour', 'Gala', 'Spectacle', 'Feast',
            'Jam', 'Happening', 'Gathering', 'Soiree', 'Headliner', 'Carnival', 'Exhibit', 'Matinee', 'Recital',
            'Spotlight'
        ],
        'loudness': [
            'Loud', 'Boom', 'Thunder', 'Blast', 'Noise', 'Roar', 'Volume', 'Crash', 'Bang', 'Clang',
            'Echo', 'Amplify', 'Deafen', 'Explosion', 'Clamor', 'Resound', 'Rumble', 'Shout', 'Scream', 'Racket',
            'Bellow', 'Reverberation', 'Raucous', 'Tumult', 'Blare', 'Sonic', 'Uproar', 'Thunderous', 'Vociferous',
            'Ear-splitting'
        ],
        'speechiness': [
            'Story', 'Talk', 'Speak', 'Narrative', 'Lyric', 'Dialogue', 'Monologue', 'Rhyme', 'Chatter', 'Discourse',
            'Spoken', 'Narrate', 'Chat', 'Prose', 'Verse', 'Recite', 'Address', 'Speech', 'Converse', 'Tale',
            'Utter', 'Vocalize', 'Proclaim', 'Soliloquy', 'Oration', 'Vernacular', 'Exclaim', 'Pronounce', 'Announce',
            'Articulate'
        ],
        'valence': [
            'Joy', 'Happy', 'Bright', 'Smile', 'Sunny', 'Bliss', 'Glee', 'Cheer', 'Glow', 'Elated',
            'Euphoria', 'Upbeat', 'Radiance', 'Exuberance', 'Mirth', 'Pleasant', 'Bubbly', 'Content', 'Jubilant',
            'Delight',
            'Cheerful', 'Jolly', 'Festive', 'Sanguine', 'Buoyant', 'Optimism', 'Ecstatic', 'Gratified', 'Lighthearted',
            'Blithe'
        ],
        'tempo': [
            'Rapid', 'Speedy', 'Quick', 'Pulse', 'Accelerate', 'Breeze', 'Dash', 'Flight', 'Hurry', 'Nimble',
            'Presto', 'Swift', 'Velocity', 'Whirl', 'Zoom', 'Bolt', 'Clip', 'Fleet', 'Gallop', 'Jog',
            'Race', 'Sprint', 'Surge', 'Thrust', 'Whisk', 'Allegro', 'Canter', 'Glide', 'Pace', 'Scurry'
        ]
    }

    title_parts = []

    # Sort features by value
    sorted_features = sorted(features.items(), key=lambda item: item[1], reverse=True)
    print(sorted_features)

    # Pick the top 3 features
    for feature, value in sorted_features[:3]:
        # Choose a random word from the themes related to the feature
        word_choice = random.choice(themes[feature])
        title_parts.append(word_choice)

    # Generate title by combining the chosen words
    title = ' '.join(title_parts)

    return title

# Example usage:
features = {
    'acousticness': 0.9,
    'danceability': 0.8,
    'energy': 0.5,
    'instrumentalness': 0.1,
    'liveness': 0.2,
    'loudness': 0.7,
    'speechiness': 0.4,
    'valence': 0.95,
    'tempo': 0.3,  # Let's assume tempo here is normalized for simplicity
}

#print(suggest_song_title(features))